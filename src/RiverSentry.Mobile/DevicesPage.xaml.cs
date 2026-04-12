namespace RiverSentry.Mobile;

public partial class DevicesPage : ContentPage
{
    public DevicesPage()
    {
        InitializeComponent();
        appWebView.LoadUrl($"{AppConfig.WebBaseUrl}/devices?shell=mobile");
    }
}
