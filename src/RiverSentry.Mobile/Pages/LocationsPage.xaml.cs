namespace RiverSentry.Mobile.Pages;

public partial class LocationsPage : ContentPage
{
    public LocationsPage()
    {
        InitializeComponent();
        appWebView.LoadUrl($"{AppConfig.WebBaseUrl}/locations?shell=mobile");
    }

    private async void OnBackTapped(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
