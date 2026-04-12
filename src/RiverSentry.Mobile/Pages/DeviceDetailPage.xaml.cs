namespace RiverSentry.Mobile.Pages;

public partial class DeviceDetailPage : ContentPage
{
    public DeviceDetailPage(Guid deviceId)
    {
        InitializeComponent();

        // Load device detail from server
        var url = $"{AppConfig.WebBaseUrl}/device/{deviceId}?shell=mobile";
        appWebView.LoadUrl(url);
    }

    private async void OnCloseTapped(object? sender, EventArgs e)
    {
        // Check if we're in a modal stack
        if (Navigation.ModalStack.Count > 0)
        {
            await Navigation.PopModalAsync();
        }
        else
        {
            // Fall back to Shell navigation if not in modal
            await Shell.Current.GoToAsync("..");
        }
    }
}
