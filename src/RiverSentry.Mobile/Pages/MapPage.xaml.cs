using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using RiverSentry.Contracts.DTOs;
using RiverSentry.Domain.Enums;
using RiverSentry.Mobile.Controls;
using RiverSentry.UI.Shared.Services;

namespace RiverSentry.Mobile.Pages;

public partial class MapPage : ContentPage
{
    private readonly IDeviceService _deviceService;
    private List<DeviceDto> _devices = [];
    private List<DeviceDto> _filteredDevices = [];
    private List<string> _families = [];
    private string? _selectedFamily;
    private bool _hasInitiallyZoomed;
    private bool _isLoading;
    private const string AllDevicesOption = "All Devices";

    public MapPage(IDeviceService deviceService)
    {
        InitializeComponent();
        _deviceService = deviceService;

        // Set initial position to Texas Hill Country (where devices are)
        DeviceMap.MoveToRegion(MapSpan.FromCenterAndRadius(
            new Location(30.05, -99.37),
            Distance.FromMiles(20)));
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Only load if we don't have devices yet
        if (_devices.Count == 0 && !_isLoading)
        {
            await LoadDevicesAsync();
        }
        // Otherwise, keep everything as-is (pins, zoom, position)
    }

    private async Task LoadDevicesAsync()
    {
        if (_isLoading) return;
        _isLoading = true;

        try
        {
            LoadingOverlay.IsVisible = true;

            _devices = (await _deviceService.GetAllDevicesAsync()).ToList();

            // Extract unique families and populate picker
            _families = _devices
                .Select(d => d.FamilyName)
                .Where(f => !string.IsNullOrEmpty(f))
                .Distinct()
                .OrderBy(f => f)
                .Select(f => f!)
                .ToList();

            PopulateFamilyPicker();

            // Initially show all devices
            _selectedFamily = null;
            _filteredDevices = _devices;
            UpdateMapPins();

            if (!_hasInitiallyZoomed && _filteredDevices.Any())
            {
                _hasInitiallyZoomed = true;
                CenterMapOnDevices();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load devices: {ex.Message}", "OK");
        }
        finally
        {
            LoadingOverlay.IsVisible = false;
            _isLoading = false;
        }
    }

    private void PopulateFamilyPicker()
    {
        FamilyPicker.Items.Clear();
        FamilyPicker.Items.Add($"All Devices ({_devices.Count})");
        foreach (var family in _families)
        {
            var count = _devices.Count(d => d.FamilyName == family);
            FamilyPicker.Items.Add($"{family} ({count})");
        }
        FamilyPicker.SelectedIndex = 0;
    }

    private void OnFamilySelected(object? sender, EventArgs e)
    {
        if (FamilyPicker.SelectedIndex < 0) return;

        if (FamilyPicker.SelectedIndex == 0)
        {
            _selectedFamily = null;
            _filteredDevices = _devices;
        }
        else
        {
            _selectedFamily = _families[FamilyPicker.SelectedIndex - 1];
            _filteredDevices = _devices.Where(d => d.FamilyName == _selectedFamily).ToList();
        }

        UpdateMapPins();
        CenterMapOnDevices();
    }

    private void UpdateMapPins()
    {
        DeviceMap.Pins.Clear();

        foreach (var device in _filteredDevices)
        {
            var pin = new CustomPin
            {
                Label = device.Name,
                Address = string.Empty,
                Location = new Location(device.Latitude, device.Longitude),
                Type = PinType.Place,
                ImageSource = ImageSource.FromFile("device.png"),
                DeviceState = device.State
            };

            var capturedDevice = device;
            pin.MarkerClicked += (s, e) =>
            {
                e.HideInfoWindow = false;
                OnPinClicked(capturedDevice);
            };

            DeviceMap.Pins.Add(pin);
        }
    }

    private void CenterMapOnDevices()
    {
        if (!_filteredDevices.Any()) return;

        var minLat = _filteredDevices.Min(d => d.Latitude);
        var maxLat = _filteredDevices.Max(d => d.Latitude);
        var minLng = _filteredDevices.Min(d => d.Longitude);
        var maxLng = _filteredDevices.Max(d => d.Longitude);

        var centerLat = (minLat + maxLat) / 2;
        var centerLng = (minLng + maxLng) / 2;

        // Calculate radius to fit all devices, zooming in as tight as possible
        var latRange = maxLat - minLat;
        var lngRange = maxLng - minLng;
        var latDistanceMeters = latRange * 111000;
        var lngDistanceMeters = lngRange * 111000 * Math.Cos(centerLat * Math.PI / 180);
        var maxDistanceMeters = Math.Max(latDistanceMeters, lngDistanceMeters);
        // Use half the span plus 10% padding, minimum 50m for max zoom
        var radiusMeters = Math.Max(maxDistanceMeters * 0.55, 50);

        DeviceMap.MoveToRegion(MapSpan.FromCenterAndRadius(
            new Location(centerLat, centerLng),
            Distance.FromMeters(radiusMeters)));
    }

    private async void OnPinClicked(DeviceDto device)
    {
        await Navigation.PushModalAsync(new DeviceDetailPage(device.Id));
    }

    private static string GetStatusText(DeviceState state) => state switch
    {
        DeviceState.Armed => "Armed",
        DeviceState.AlarmWater => "⚠️ Water Alarm!",
        DeviceState.AlarmUpstream => "⚠️ Upstream Alarm!",
        DeviceState.AlarmSilent => "Silent Alarm",
        DeviceState.AlarmDrill => "Drill / Test",
        DeviceState.Offline => "Offline",
        _ => "Unknown"
    };

    private void OnCenterButtonClicked(object? sender, EventArgs e) => CenterMapOnDevices();

}
