using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;

namespace RiverSentry.Mobile.Services;

/// <summary>
/// Service to send local notifications for alarm simulation and real alerts.
/// Works on both Android and iOS.
/// </summary>
public class AlarmNotificationService
{
    private int _notificationId = 100;

    public async Task SendAlarmNotificationAsync(string title, string message, string alarmType)
    {
        // Request permission if needed (iOS requires this, Android 13+ also)
        if (!await LocalNotificationCenter.Current.AreNotificationsEnabled())
        {
            await LocalNotificationCenter.Current.RequestNotificationPermission();
        }

        var notification = new NotificationRequest
        {
            NotificationId = _notificationId++,
            Title = title,
            Description = message,
            CategoryType = NotificationCategoryType.Alarm,
            Sound = GetAlarmSound(alarmType),
            Android = new AndroidOptions
            {
                ChannelId = "flood_alerts",
                Priority = AndroidPriority.High,
                VisibilityType = AndroidVisibilityType.Public,
                Ongoing = false,
                AutoCancel = true,
                VibrationPattern = GetVibrationPattern(alarmType),
            },
            iOS = new Plugin.LocalNotification.iOSOption.iOSOptions
            {
                PlayForegroundSound = true,
            }
        };

        await LocalNotificationCenter.Current.Show(notification);
    }

    private static string GetAlarmSound(string alarmType)
    {
        // Use system alarm sound for urgent alerts, default for test
        return alarmType switch
        {
            "water" or "upstream" => "Default", // System will use alarm channel sound
            _ => "Default"
        };
    }

    public async Task SendWaterAlarmAsync(string deviceName)
    {
        await SendAlarmNotificationAsync(
            "⚠️ WATER ALARM",
            $"{deviceName} has detected rising water levels!",
            "water");
    }

    public async Task SendUpstreamAlarmAsync(string deviceName)
    {
        await SendAlarmNotificationAsync(
            "⚠️ UPSTREAM ALARM", 
            $"{deviceName} received upstream flood warning!",
            "upstream");
    }

    public async Task SendTestAlarmAsync()
    {
        await SendAlarmNotificationAsync(
            "🔔 Test Alarm",
            "This is a test notification from River Sentry.",
            "test");
    }

    private static long[] GetVibrationPattern(string alarmType)
    {
        return alarmType switch
        {
            "water" => new long[] { 0, 500, 200, 500, 200, 500 },      // Urgent pattern
            "upstream" => new long[] { 0, 300, 200, 300, 200, 300 },  // Warning pattern  
            _ => new long[] { 0, 250, 250, 250 }                       // Default pattern
        };
    }
}
