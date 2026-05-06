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

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Hide back button when shown as a top-level tab (no navigation stack to go back to)
        var navStack = Shell.Current?.Navigation?.NavigationStack;
        BackButton.IsVisible = navStack != null && navStack.Count > 1;
    }

    private async void OnBackTapped(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnWebsiteTapped(object? sender, EventArgs e)
    {
        await Launcher.OpenAsync("https://www.riversentry.com");
    }

    private async void OnEmailTapped(object? sender, EventArgs e)
    {
        await Launcher.OpenAsync("mailto:sales@riversentry.com");
    }

    private async void OnPhoneTapped(object? sender, EventArgs e)
    {
        await Launcher.OpenAsync("tel:+15129344441");
    }

    private async void OnWarrantyTapped(object? sender, EventArgs e)
    {
        await Launcher.OpenAsync("https://www.riversentry.com/support");
    }
}
