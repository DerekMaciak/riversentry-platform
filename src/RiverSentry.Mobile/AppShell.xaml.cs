using RiverSentry.Mobile.Pages;

namespace RiverSentry.Mobile;

public partial class AppShell : Shell
{
    public AppShell(MapPage mapPage)
    {
        InitializeComponent();

        // Set the singleton MapPage instance to preserve state
        mapShellContent.Content = mapPage;
    }
}
