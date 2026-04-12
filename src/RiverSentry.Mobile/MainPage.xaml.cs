namespace RiverSentry.Mobile;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        appWebView.LoadUrl($"{AppConfig.WebBaseUrl}/?shell=mobile");
    }
}
