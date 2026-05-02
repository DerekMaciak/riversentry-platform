using Plugin.LocalNotification;
using RiverSentry.Mobile.Services;

namespace RiverSentry.Mobile;

public partial class App : Application
{
    private readonly AppShell _appShell;
    private readonly AlarmNotificationService _alarmService;

    public App(AppShell appShell, AlarmNotificationService alarmService)
    {
        InitializeComponent();
        _appShell = appShell;
        _alarmService = alarmService;

        // Stop vibration/sound when user interacts with the notification
        LocalNotificationCenter.Current.NotificationActionTapped += OnNotificationActionTapped;
    }

    private void OnNotificationActionTapped(Plugin.LocalNotification.EventArgs.NotificationActionEventArgs e)
    {
        _alarmService.StopVibration();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(_appShell) { Title = "River Sentry" };
    }
}
