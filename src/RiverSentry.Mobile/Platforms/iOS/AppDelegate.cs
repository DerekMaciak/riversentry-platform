using Foundation;
using UIKit;
using UserNotifications;

namespace RiverSentry.Mobile;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override bool FinishedLaunching(UIApplication application, NSDictionary? launchOptions)
    {
        // Request notification permissions including critical alerts (amber alert style)
        // Note: CriticalAlert requires an Apple entitlement - falls back to normal alerts if not granted
        UNUserNotificationCenter.Current.RequestAuthorization(
            UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound | UNAuthorizationOptions.CriticalAlert,
            (granted, error) =>
            {
                if (granted)
                {
                    InvokeOnMainThread(() =>
                    {
                        UIApplication.SharedApplication.RegisterForRemoteNotifications();
                    });
                }
            });

        return base.FinishedLaunching(application, launchOptions!);
    }
}
