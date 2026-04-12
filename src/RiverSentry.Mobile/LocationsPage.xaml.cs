namespace RiverSentry.Mobile;

public partial class LocationsPage : ContentPage
{
    public LocationsPage()
    {
        InitializeComponent();
        appWebView.LoadUrl($"{AppConfig.WebBaseUrl}/locations?shell=mobile");
    }
}
