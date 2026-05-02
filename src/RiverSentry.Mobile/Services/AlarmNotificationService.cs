using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;

namespace RiverSentry.Mobile.Services;

/// <summary>
/// Service to send amber-alert-style local notifications for flood alarms.
/// Plays alarm sound natively via AVAudioPlayer (iOS) or notification channel (Android),
/// vibrates aggressively, and shows high-priority notification.
/// </summary>
public partial class AlarmNotificationService
{
    private int _notificationId = 100;
    private CancellationTokenSource? _alarmCts;
    private bool _isAlarming;

    public bool IsAlarming => _isAlarming;

    public async Task SendAlarmNotificationAsync(string title, string message, string alarmType)
    {
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

        if (isUrgent)
        {
            _ = RunAlarmAsync(alarmType);
        }
        else
        {
            VibrateOnce();
        }
    }

    private async Task RunAlarmAsync(string alarmType)
    {
        StopAlarm();
        _alarmCts = new CancellationTokenSource();
        _isAlarming = true;
        var ct = _alarmCts.Token;

        var vibeDuration = alarmType == "water"
            ? TimeSpan.FromMilliseconds(800)
            : TimeSpan.FromMilliseconds(500);

        // Start looping audio
        StartAlarmAudio();

        try
        {
            for (var i = 0; i < 15 && !ct.IsCancellationRequested; i++)
            {
                Vibration.Default.Vibrate(vibeDuration);
                await Task.Delay(1000, ct);
            }
        }
        catch (OperationCanceledException) { }
        catch (FeatureNotSupportedException) { }
        finally
        {
            StopAlarmAudio();
            _isAlarming = false;
        }
    }

    private static void VibrateOnce()
    {
        try { Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(500)); }
        catch (FeatureNotSupportedException) { }
    }

    public void StopAlarm()
    {
        _alarmCts?.Cancel();
        _alarmCts?.Dispose();
        _alarmCts = null;
        _isAlarming = false;
        StopAlarmAudio();
        try { Vibration.Default.Cancel(); } catch { }
    }

    // Platform-specific audio methods — implemented via partial methods
    partial void StartAlarmAudio();
    partial void StopAlarmAudio();

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
        return alarmType switch
        {
            "water" => [0, 1000, 200, 1000, 200, 1000, 200, 1000, 200, 1000],
            "upstream" => [0, 800, 200, 800, 200, 800, 200, 800, 200, 800],
            _ => [0, 500, 250, 500]
        };
    }
}
