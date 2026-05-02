using CoreGraphics;
using CoreLocation;
using Foundation;
using MapKit;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Maps.Handlers;
using Microsoft.Maui.Platform;
using RiverSentry.Domain.Enums;
using RiverSentry.Mobile.Controls;
using UIKit;
using IMap = Microsoft.Maui.Maps.IMap;

namespace RiverSentry.Mobile.Platforms.iOS.Handlers;

/// <summary>
/// Custom annotation that stores the pin image.
/// </summary>
public class CustomAnnotation : MKPointAnnotation
{
    public UIImage? Image { get; set; }
    public CustomPin? Pin { get; set; }
}

/// <summary>
/// Custom map handler for iOS that supports custom pin images with status text badges.
/// </summary>
public class CustomMapHandler : MapHandler
{
    private MKMapView? _mapView;
    private UIImage? _cachedDeviceIcon;
    private readonly Dictionary<DeviceState, UIImage> _compositeCache = new();

    public static new IPropertyMapper<IMap, IMapHandler> Mapper = new PropertyMapper<IMap, IMapHandler>(MapHandler.Mapper)
    {
        [nameof(IMap.Pins)] = MapPins
    };

    public CustomMapHandler() : base(Mapper, CommandMapper)
    {
    }

    public override void SetVirtualView(IView view)
    {
        base.SetVirtualView(view);

        if (PlatformView is MKMapView mapView && _mapView == null)
        {
            _mapView = mapView;
            mapView.GetViewForAnnotation = GetViewForAnnotation;
            mapView.DidSelectAnnotationView += OnAnnotationViewSelected;
        }
    }

    private void OnAnnotationViewSelected(object? sender, MKAnnotationViewEventArgs e)
    {
        if (e.View.Annotation is CustomAnnotation customAnnotation && customAnnotation.Pin != null)
        {
            _mapView?.DeselectAnnotation(customAnnotation, false);
            customAnnotation.Pin.SendMarkerClick();
        }
    }

    private static new void MapPins(IMapHandler handler, IMap map)
    {
        if (handler is CustomMapHandler customHandler)
        {
            customHandler.UpdatePins(map);
        }
    }

    private MKAnnotationView? GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
    {
        if (annotation is CustomAnnotation customAnnotation)
        {
            const string identifier = "CustomPin";
            var annotationView = mapView.DequeueReusableAnnotation(identifier) as MKAnnotationView
                ?? new MKAnnotationView(annotation, identifier);

            annotationView.Annotation = annotation;
            annotationView.CanShowCallout = false;

            if (customAnnotation.Image != null)
            {
                // Image is already the fully composited marker (icon + badge) from cache
                annotationView.Image = customAnnotation.Image;
                annotationView.CenterOffset = new CGPoint(0, -25);
            }

            return annotationView;
        }

        return null;
    }

    private static UIImage CreateCompositeMarker(UIImage iconImage, DeviceState deviceState)
    {
        const float iconSize = 56f;
        const float spacing = 2f;
        const float statusFontSize = 12f;
        const float badgePaddingH = 6f;
        const float badgePaddingV = 3f;

        var statusFont = UIFont.BoldSystemFontOfSize(statusFontSize);
        var statusColor = GetStatusColor(deviceState);
        var statusText = GetStatusText(deviceState);

        var statusAttrs = new UIStringAttributes { Font = statusFont };
        var statusSize = new NSString(statusText).GetSizeUsingAttributes(statusAttrs);

        var badgeWidth = statusSize.Width + badgePaddingH * 2;
        var badgeHeight = statusSize.Height + badgePaddingV * 2;
        var width = (float)Math.Max(iconSize, badgeWidth);
        var height = iconSize + spacing + (float)badgeHeight;

        var renderer = new UIGraphicsImageRenderer(new CGSize(width, height));

        return renderer.CreateImage(context =>
        {
            var centerX = width / 2f;

            // Draw icon centered at top
            var iconRect = new CGRect(centerX - iconSize / 2f, 0, iconSize, iconSize);
            iconImage.Draw(iconRect);

            // Draw status badge below icon
            var badgeTop = iconSize + spacing;
            var badgeLeft = centerX - (float)badgeWidth / 2f;
            var badgeRect = new CGRect(badgeLeft, badgeTop, badgeWidth, badgeHeight);
            var badgePath = UIBezierPath.FromRoundedRect(badgeRect, (float)badgeHeight / 2f);
            statusColor.SetFill();
            badgePath.Fill();

            // Draw status text centered in badge
            var statusDrawAttrs = new UIStringAttributes
            {
                Font = statusFont,
                ForegroundColor = UIColor.White
            };
            var statusX = centerX - statusSize.Width / 2f;
            var statusY = badgeTop + badgePaddingV;
            new NSString(statusText).DrawString(new CGPoint(statusX, statusY), statusDrawAttrs);
        });
    }


    private static string GetStatusText(DeviceState state) => state switch
    {
        DeviceState.Armed => "Armed",
        DeviceState.AlarmWater => "Water",
        DeviceState.AlarmUpstream => "Upstream",
        DeviceState.AlarmSilent => "Silent",
        DeviceState.AlarmDrill => "Drill",
        DeviceState.Offline => "Offline",
        _ => "Unknown"
    };

    private static UIColor GetStatusColor(DeviceState state) => state switch
    {
        DeviceState.Armed => UIColor.FromRGB(76, 175, 80),           // Green
        DeviceState.AlarmWater => UIColor.FromRGB(244, 67, 54),      // Red
        DeviceState.AlarmUpstream => UIColor.FromRGB(255, 152, 0),   // Orange
        DeviceState.AlarmSilent => UIColor.FromRGB(156, 39, 176),    // Purple
        DeviceState.AlarmDrill => UIColor.FromRGB(255, 235, 59),     // Yellow
        DeviceState.Offline => UIColor.FromRGB(158, 158, 158),       // Gray
        _ => UIColor.White
    };

    private CancellationTokenSource? _pinUpdateCts;

    private async void UpdatePins(IMap map)
    {
        // Cancel any pending update — this is the key to avoiding freezes.
        // When pins are removed/added one by one, each change fires this method.
        // The debounce ensures only the LAST call actually does work.
        _pinUpdateCts?.Cancel();
        _pinUpdateCts = new CancellationTokenSource();
        var token = _pinUpdateCts.Token;

        try
        {
            // Wait briefly to let all rapid-fire collection changes finish
            await Task.Delay(30, token);
            if (token.IsCancellationRequested) return;

            if (PlatformView is not MKMapView mapView)
                return;

            // Snapshot the pins we need to show
            var pinsToAdd = map.Pins.OfType<CustomPin>().ToList();

            // Remove all existing custom annotations in one batch
            var existingAnnotations = mapView.Annotations?.OfType<CustomAnnotation>().ToArray();
            if (existingAnnotations is { Length: > 0 })
            {
                mapView.RemoveAnnotations(existingAnnotations);
            }

            // Load the device icon once and cache it
            if (_cachedDeviceIcon == null)
            {
                var firstPin = pinsToAdd.FirstOrDefault(p => p.ImageSource != null);
                if (firstPin?.ImageSource != null)
                {
                    _cachedDeviceIcon = await LoadImageFromSourceAsync(firstPin.ImageSource);
                }
            }

            // Build annotations synchronously using cached images
            var annotations = new CustomAnnotation[pinsToAdd.Count];
            for (var i = 0; i < pinsToAdd.Count; i++)
            {
                var pin = pinsToAdd[i];
                annotations[i] = new CustomAnnotation
                {
                    Title = pin.Label,
                    Subtitle = pin.Address,
                    Coordinate = new CLLocationCoordinate2D(pin.Location.Latitude, pin.Location.Longitude),
                    Pin = pin,
                    Image = GetCachedCompositeMarker(pin.DeviceState)
                };
            }

            if (token.IsCancellationRequested) return;

            // Add all annotations in one batch on the UI thread
            MainThread.BeginInvokeOnMainThread(() =>
            {
                mapView.AddAnnotations(annotations);
            });
        }
        catch (TaskCanceledException)
        {
            // Expected when debouncing — a newer update superseded this one
        }
    }


    private UIImage? GetCachedCompositeMarker(DeviceState state)
    {
        if (_cachedDeviceIcon == null)
            return null;

        if (!_compositeCache.TryGetValue(state, out var composite))
        {
            composite = CreateCompositeMarker(_cachedDeviceIcon, state);
            _compositeCache[state] = composite;
        }

        return composite;
    }

    private async Task<UIImage?> LoadImageFromSourceAsync(ImageSource imageSource)
    {
        if (MauiContext == null)
            return null;

        try
        {
            var result = await imageSource.GetPlatformImageAsync(MauiContext);
            return result?.Value;
        }
        catch
        {
            return null;
        }
    }
}