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

            if (customAnnotation.Image != null && customAnnotation.Pin != null)
            {
                var compositeImage = CreateCompositeMarker(
                    customAnnotation.Image,
                    customAnnotation.Pin.DeviceState);
                annotationView.Image = compositeImage;
                annotationView.CenterOffset = new CGPoint(0, -20);
            }

            return annotationView;
        }

        return null;
    }

    private static UIImage CreateCompositeMarker(UIImage iconImage, DeviceState deviceState)
    {
        const float iconSize = 40f;
        const float spacing = 2f;
        const float statusFontSize = 9f;
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

    private async void UpdatePins(IMap map)
    {
        if (PlatformView is not MKMapView mapView)
            return;

        var existingAnnotations = mapView.Annotations;
        if (existingAnnotations != null)
        {
            foreach (var annotation in existingAnnotations.OfType<CustomAnnotation>())
            {
                mapView.RemoveAnnotation(annotation);
            }
        }

        foreach (var pin in map.Pins)
        {
            if (pin is CustomPin customPin)
            {
                await AddCustomPinAsync(customPin, mapView);
            }
        }
    }

    private async Task AddCustomPinAsync(CustomPin pin, MKMapView mapView)
    {
        var annotation = new CustomAnnotation
        {
            Title = pin.Label,
            Subtitle = pin.Address,
            Coordinate = new CLLocationCoordinate2D(pin.Location.Latitude, pin.Location.Longitude),
            Pin = pin
        };

        if (pin.ImageSource != null)
        {
            try
            {
                var image = await LoadImageFromSourceAsync(pin.ImageSource);
                annotation.Image = image;
            }
            catch
            {
                // Use default annotation if image fails to load
            }
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            mapView.AddAnnotation(annotation);
        });
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