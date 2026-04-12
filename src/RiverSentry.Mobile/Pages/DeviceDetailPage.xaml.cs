namespace RiverSentry.Mobile.Pages;

public partial class DeviceDetailPage : ContentPage
{
    public static Guid CurrentDeviceId { get; private set; }

    public DeviceDetailPage(Guid deviceId)
    {
        CurrentDeviceId = deviceId;
        InitializeComponent();
    }

    private async void OnCloseTapped(object? sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
