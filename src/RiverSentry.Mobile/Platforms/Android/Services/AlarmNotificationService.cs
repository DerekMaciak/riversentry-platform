namespace RiverSentry.Mobile.Services;

public partial class AlarmNotificationService
{
    // Android alarm sound is handled by the notification channel (flood_alerts).
    // No additional audio playback needed.
    partial void StartAlarmAudio() { }
    partial void StopAlarmAudio() { }
}
