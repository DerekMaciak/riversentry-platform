using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;

namespace RiverSentry.Mobile.Services;

/// <summary>
/// Service to send amber-alert-style local notifications for flood alarms.
/// Plays alarm sound, vibrates aggressively, and shows high-priority notification.
/// </summary>
public class AlarmNotificationService
{
    private int _notificationId = 100;
    private CancellationTokenSource? _vibrationCts;

    public async Task SendAlarmNotificationAsync(string title, string message, string alarmType)
    {
        // Request permission if needed (iOS requires this, Android 13+ also)
        if (!await LocalNotificationCenter.Current.AreNotificationsEnabled())
        {
            await LocalNotificationCenter.Current.RequestNotificationPermission();
        }

        var isUrgent = alarmType is "water" or "upstream";

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
                Ongoing = isUrgent,
                AutoCancel = true,
                VibrationPattern = GetVibrationPattern(alarmType),
            },
            iOS = new Plugin.LocalNotification.iOSOption.iOSOptions
            {
                PlayForegroundSound = true,
            }
        };

        await LocalNotificationCenter.Current.Show(notification);

        // Start aggressive vibration pattern for urgent alerts
        if (isUrgent)
        {
            _ = VibrateRepeatedlyAsync(alarmType);
        }
        else
        {
            VibrateOnce();
        }
    }

    /// <summary>
    /// Vibrates repeatedly like an amber alert for urgent flood alarms.
    /// </summary>
    private async Task VibrateRepeatedlyAsync(string alarmType)
    {
        StopVibration();
        _vibrationCts = new CancellationTokenSource();
        var ct = _vibrationCts.Token;

        var duration = alarmType == "water"
            ? TimeSpan.FromMilliseconds(800)
            : TimeSpan.FromMilliseconds(500);

        try
        {
            // Vibrate aggressively for 15 seconds (like WEA alerts)
            for (var i = 0; i < 15 && !ct.IsCancellationRequested; i++)
            {
                Vibration.Default.Vibrate(duration);
                await Task.Delay(1000, ct);
            }
        }
        catch (OperationCanceledException) { }
        catch (FeatureNotSupportedException) { }
    }

    private static void VibrateOnce()
    {
        try
        {
            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(500));
        }
        catch (FeatureNotSupportedException) { }
    }

    public void StopVibration()
    {
        _vibrationCts?.Cancel();
        _vibrationCts?.Dispose();
        _vibrationCts = null;
        try { Vibration.Default.Cancel(); } catch { }
    }

    private static string GetAlarmSound(string alarmType)
    {
        return alarmType switch
        {
            // Use system alarm sound for urgent alerts
            "water" or "upstream" => "Default",
            _ => "Default"
        };
    }

    public async Task SendWaterAlarmAsync(string deviceName)
    {
        await SendAlarmNotificationAsync(
            "⚠️ WATER ALARM",
            $"{deviceName} has detected rising water levels! Seek higher ground immediately.",
            "water");
    }

    public async Task SendUpstreamAlarmAsync(string deviceName)
    {
        await SendAlarmNotificationAsync(
            "⚠️ UPSTREAM ALARM",
            $"{deviceName} received upstream flood warning! Take action now.",
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
        // Aggressive amber-alert-style vibration patterns
        return alarmType switch
        {
            "water" => [0, 1000, 200, 1000, 200, 1000, 200, 1000, 200, 1000],
            "upstream" => [0, 800, 200, 800, 200, 800, 200, 800, 200, 800],
            _ => [0, 500, 250, 500]
        };
    }
}
