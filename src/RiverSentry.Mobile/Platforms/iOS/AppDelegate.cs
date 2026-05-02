using Foundation;
using RiverSentry.Mobile.Services;
using UIKit;
using UserNotifications;

namespace RiverSentry.Mobile;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override bool FinishedLaunching(UIApplication application, NSDictionary? launchOptions)
    {
        var result = base.FinishedLaunching(application, launchOptions!);

        // Wrap the plugin's delegate with ours so we can intercept dismiss actions
        var pluginDelegate = UNUserNotificationCenter.Current.Delegate;
        UNUserNotificationCenter.Current.Delegate = new AlarmNotificationDelegate(pluginDelegate);

        // Register alarm category with CustomDismissAction so iOS tells us
        // when the user swipes away the notification
        var alarmCategory = UNNotificationCategory.FromIdentifier(
            "Alarm",  // Must match Plugin.LocalNotification's CategoryType.Alarm identifier
            Array.Empty<UNNotificationAction>(),
            Array.Empty<string>(),
            UNNotificationCategoryOptions.CustomDismissAction);
        UNUserNotificationCenter.Current.SetNotificationCategories(
            new NSSet<UNNotificationCategory>(alarmCategory));

        // Request notification permissions including critical alerts (amber alert style)
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

        return result;
    }
}

/// <summary>
/// Wraps the plugin's UNUserNotificationCenterDelegate to add dismiss handling
/// while preserving all plugin behavior.
/// </summary>
public class AlarmNotificationDelegate : NSObject, IUNUserNotificationCenterDelegate
{
    private readonly IUNUserNotificationCenterDelegate? _inner;

    public AlarmNotificationDelegate(IUNUserNotificationCenterDelegate? inner)
    {
        _inner = inner;
    }

    [Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
    public void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
    {
        if (_inner != null)
        {
            _inner.WillPresentNotification(center, notification, completionHandler);
        }
        else
        {
            completionHandler(UNNotificationPresentationOptions.Banner | UNNotificationPresentationOptions.Sound | UNNotificationPresentationOptions.Badge);
        }
    }

    [Export("userNotificationCenter:didReceiveNotificationResponse:withCompletionHandler:")]
    public void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
    {
        // Stop alarm on tap OR dismiss
        var alarmService = IPlatformApplication.Current?.Services.GetService<AlarmNotificationService>();
        alarmService?.StopAlarm();

        // Forward to plugin so NotificationActionTapped still fires
        if (_inner != null)
        {
            _inner.DidReceiveNotificationResponse(center, response, completionHandler);
        }
        else
        {
            completionHandler();
        }
    }
}
