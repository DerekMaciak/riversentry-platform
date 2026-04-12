using RiverSentry.Contracts.DTOs;
using RiverSentry.Mobile.Services;

namespace RiverSentry.Mobile.Pages;

public partial class DeviceDetailPage : ContentPage
{
    public DeviceDetailPage(DeviceDto device, DeviceNavigationService navService)
    {
        // Set the device in the service BEFORE initializing Blazor
        navService.CurrentDevice = device;
        navService.CloseModal = async () => await Navigation.PopModalAsync();

        InitializeComponent();
    }

    private async void OnCloseTapped(object? sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
