namespace RiverSentry.Mobile;

public partial class AlertsPage : ContentPage
{
    public AlertsPage()
    {
        InitializeComponent();
        appWebView.LoadUrl($"{AppConfig.WebBaseUrl}/alarms?shell=mobile");
    }
}
