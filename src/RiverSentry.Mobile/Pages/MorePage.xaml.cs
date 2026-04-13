namespace RiverSentry.Mobile.Pages;

public partial class MorePage : ContentPage
{
    public MorePage()
    {
        InitializeComponent();
    }

    private async void OnLocationsTapped(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("locations");
    }

    private async void OnAboutTapped(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("about");
    }
}
