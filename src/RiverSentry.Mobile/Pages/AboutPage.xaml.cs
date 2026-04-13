namespace RiverSentry.Mobile.Pages;

public partial class AboutPage : ContentPage
{
    public AboutPage()
    {
        InitializeComponent();

        // Get app version from assembly
        var version = AppInfo.Current.VersionString;
        var build = AppInfo.Current.BuildString;
        versionLabel.Text = $"{version} ({build})";
    }

    private async void OnBackTapped(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnWebsiteTapped(object? sender, EventArgs e)
    {
        await Launcher.OpenAsync("https://www.riversentry.com");
    }
}
