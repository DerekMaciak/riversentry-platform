using Android.App;
using Android.Content.PM;
using Android.Media;
using Android.OS;
using Android.Provider;

namespace RiverSentry.Mobile;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        CreateNotificationChannel();
    }

    private void CreateNotificationChannel()
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
#pragma warning disable CA1416 // Validate platform compatibility - guarded by API level check above
            // Use system alarm sound for urgent flood alerts
            var alarmSound = RingtoneManager.GetDefaultUri(RingtoneType.Alarm);
            var audioAttributes = new AudioAttributes.Builder()
                .SetUsage(AudioUsageKind.Alarm)!
                .SetContentType(AudioContentType.Sonification)!
                .Build();

            var channel = new NotificationChannel(
                "flood_alerts",
                "Flood Alerts",
                NotificationImportance.High)
            {
                Description = "Critical flood alert notifications"
            };

            channel.EnableVibration(true);
            channel.EnableLights(true);
            channel.SetSound(alarmSound, audioAttributes);

            var notificationManager = GetSystemService(NotificationService) as NotificationManager;
            notificationManager?.CreateNotificationChannel(channel);
#pragma warning restore CA1416
        }
    }
}
