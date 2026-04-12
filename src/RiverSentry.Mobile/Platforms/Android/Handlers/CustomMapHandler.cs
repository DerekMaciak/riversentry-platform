using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Graphics.Drawables;
using RiverSentry.Domain.Enums;
using RiverSentry.Mobile.Controls;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Maps.Handlers;
using Microsoft.Maui.Platform;
using IMap = Microsoft.Maui.Maps.IMap;

namespace RiverSentry.Mobile.Platforms.Android.Handlers;

/// <summary>
/// Custom map handler for Android that supports custom pin images.
/// </summary>
public class CustomMapHandler : MapHandler
{
    private readonly Dictionary<string, Marker> _markers = new();
    private readonly Dictionary<string, CustomPin> _markerPins = new();

    // Static cache - persists across handler instances
    private static readonly Dictionary<DeviceState, BitmapDescriptor> _cachedMarkerDescriptors = new();
    private static Bitmap? _cachedDeviceBitmap;
    private static bool _descriptorsInitialized;

    private GoogleMap? _googleMap;
    private bool _isUpdating;

    public static new IPropertyMapper<IMap, IMapHandler> Mapper = new PropertyMapper<IMap, IMapHandler>(MapHandler.Mapper)
    {
        [nameof(IMap.Pins)] = MapPins
    };

    public CustomMapHandler() : base(Mapper, CommandMapper)
    {
    }

    protected override void ConnectHandler(MapView platformView)
    {
        base.ConnectHandler(platformView);

        // Clear any stale state
        _googleMap = null;
        foreach (var marker in _markers.Values)
        {
            marker.Remove();
        }
        _markers.Clear();
        _markerPins.Clear();

        platformView.GetMapAsync(new MapReadyCallback(this));
    }

    protected override void DisconnectHandler(MapView platformView)
    {
        // Clean up markers only (keep bitmap cache for fast reconnect)
        foreach (var marker in _markers.Values)
        {
            marker.Remove();
        }
        _markers.Clear();
        _markerPins.Clear();

        // Unsubscribe from events
        if (_googleMap != null)
        {
            _googleMap.MarkerClick -= OnMarkerClick;
            _googleMap = null;
        }

        // Note: Keep _cachedDeviceBitmap and _cachedCompositeMarkers
        // for faster reconnection when navigating back

        base.DisconnectHandler(platformView);
    }

    private class MapReadyCallback : Java.Lang.Object, IOnMapReadyCallback
    {
        private readonly CustomMapHandler _handler;

        public MapReadyCallback(CustomMapHandler handler)
        {
            _handler = handler;
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            _handler._googleMap = googleMap;

            // Set up marker click listener to trigger MAUI Pin.MarkerClicked event
            googleMap.MarkerClick += _handler.OnMarkerClick;

            _handler.UpdateCustomPins();
        }
    }

    private void OnMarkerClick(object? sender, GoogleMap.MarkerClickEventArgs e)
    {
        if (e.Marker != null)
        {
            var key = $"{e.Marker.Position.Latitude}_{e.Marker.Position.Longitude}";
            if (_markerPins.TryGetValue(key, out var pin))
            {
                // Trigger the MarkerClicked event on the MAUI Pin
                pin.SendMarkerClick();

                // Return handled = true to prevent showing the info window
                e.Handled = true;
            }
        }
    }

    private static new void MapPins(IMapHandler handler, IMap map)
    {
        if (handler is CustomMapHandler customHandler)
        {
            customHandler.UpdateCustomPins();
        }
    }

    private void UpdateCustomPins()
    {
        if (_googleMap == null || VirtualView is not IMap map || MauiContext == null || _isUpdating)
            return;

        _isUpdating = true;

        try
        {
            // Clear existing markers
            foreach (var marker in _markers.Values)
            {
                marker.Remove();
            }
            _markers.Clear();
            _markerPins.Clear();

            var customPins = map.Pins.OfType<CustomPin>().ToList();
            if (!customPins.Any()) return;

            // Ensure descriptors are ready (only runs once due to static flag)
            EnsureDescriptorsReady(customPins);

            // Add all markers
            foreach (var pin in customPins)
            {
                var markerOptions = new MarkerOptions();
                markerOptions.SetPosition(new LatLng(pin.Location.Latitude, pin.Location.Longitude));
                markerOptions.SetTitle(pin.Label);
                markerOptions.SetSnippet(pin.Address);

                if (_cachedMarkerDescriptors.TryGetValue(pin.DeviceState, out var descriptor))
                {
                    markerOptions.SetIcon(descriptor);
                    markerOptions.Anchor(0.5f, 0.35f);
                }

                var marker = _googleMap?.AddMarker(markerOptions);
                if (marker != null)
                {
                    var key = $"{pin.Location.Latitude}_{pin.Location.Longitude}";
                    _markers[key] = marker;
                    _markerPins[key] = pin;
                }
            }
        }
        finally
        {
            _isUpdating = false;
        }
    }

    private void EnsureDescriptorsReady(List<CustomPin> customPins)
    {
        // If already initialized, nothing to do
        if (_descriptorsInitialized) return;

        var neededStates = customPins.Select(p => p.DeviceState).Distinct();

        foreach (var state in neededStates)
        {
            if (_cachedMarkerDescriptors.ContainsKey(state)) continue;

            // Load device bitmap if needed
            if (_cachedDeviceBitmap == null && MauiContext?.Context != null)
            {
                var context = MauiContext.Context;
                var resId = context.Resources?.GetIdentifier("device", "drawable", context.PackageName) ?? 0;
                if (resId != 0)
                {
                    var options = new BitmapFactory.Options { InSampleSize = 16 };
                    _cachedDeviceBitmap = BitmapFactory.DecodeResource(context.Resources, resId, options);
                }
            }

            if (_cachedDeviceBitmap == null) continue;

            var composite = CreateCompositeMarker(_cachedDeviceBitmap, "", "", state);
            if (composite != null)
            {
                _cachedMarkerDescriptors[state] = BitmapDescriptorFactory.FromBitmap(composite);
                composite.Recycle();
            }
        }

        _descriptorsInitialized = true;
    }

    private Bitmap? CreateCompositeMarker(Bitmap iconBitmap, string label, string status, DeviceState deviceState)
    {
        const int iconSize = 120;
        const int dotSize = 28;
        const int spacing = 6;

        var width = iconSize;
        var height = iconSize + spacing + dotSize;

        var result = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888!);
        if (result == null) return null;

        using var canvas = new Canvas(result);

        // Draw device icon scaled to fit
        var srcRect = new global::Android.Graphics.Rect(0, 0, iconBitmap.Width, iconBitmap.Height);
        var dstRect = new global::Android.Graphics.Rect(0, 0, iconSize, iconSize);
        using var paint = new global::Android.Graphics.Paint(PaintFlags.AntiAlias | PaintFlags.FilterBitmap);
        canvas.DrawBitmap(iconBitmap, srcRect, dstRect, paint);

        // Draw status dot centered below icon
        var statusColor = GetStatusColor(deviceState);
        using var dotPaint = new global::Android.Graphics.Paint(PaintFlags.AntiAlias) { Color = statusColor };
        dotPaint.SetStyle(global::Android.Graphics.Paint.Style.Fill);
        canvas.DrawCircle(width / 2f, iconSize + spacing + dotSize / 2f, dotSize / 2f, dotPaint);

        return result;
    }

    private static global::Android.Graphics.Color GetStatusColor(DeviceState state) => state switch
    {
        DeviceState.Armed => new global::Android.Graphics.Color(76, 175, 80),       // Green
        DeviceState.AlarmWater => new global::Android.Graphics.Color(244, 67, 54),  // Red
        DeviceState.AlarmUpstream => new global::Android.Graphics.Color(255, 152, 0),  // Orange
        DeviceState.AlarmSilent => new global::Android.Graphics.Color(156, 39, 176),   // Purple
        DeviceState.AlarmDrill => new global::Android.Graphics.Color(255, 235, 59),    // Yellow
        DeviceState.Offline => new global::Android.Graphics.Color(158, 158, 158),      // Gray
        _ => new global::Android.Graphics.Color(255, 255, 255)                         // White
    };

    private Task<Bitmap?> LoadBitmapFromImageSourceAsync(ImageSource imageSource)
    {
        // Return cached bitmap if available
        if (_cachedDeviceBitmap != null && !_cachedDeviceBitmap.IsRecycled)
            return Task.FromResult<Bitmap?>(_cachedDeviceBitmap);

        if (MauiContext?.Context == null)
            return Task.FromResult<Bitmap?>(null);

        try
        {
            // Load directly from Android resources (most reliable)
            var context = MauiContext.Context;
            var resId = context.Resources?.GetIdentifier("device", "drawable", context.PackageName) ?? 0;
            if (resId != 0)
            {
                // Use options to downsample the large image
                var options = new BitmapFactory.Options
                {
                    InSampleSize = 8 // Load at 1/8 size to save memory (1024->128px)
                };
                _cachedDeviceBitmap = BitmapFactory.DecodeResource(context.Resources, resId, options);
                return Task.FromResult<Bitmap?>(_cachedDeviceBitmap);
            }
        }
        catch
        {
            // Give up
        }

        return Task.FromResult<Bitmap?>(null);
    }
}
