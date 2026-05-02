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
            // Use system alarm sound for urgent flood alerts (amber alert style)
            var alarmSound = RingtoneManager.GetDefaultUri(RingtoneType.Alarm);
            var audioAttributes = new AudioAttributes.Builder()
                .SetUsage(AudioUsageKind.Alarm)!
                .SetContentType(AudioContentType.Sonification)!
                .Build();

            var channel = new NotificationChannel(
                "flood_alerts",
                "Flood Alerts",
                NotificationImportance.Max)  // Max importance for heads-up display
            {
                Description = "Critical flood alert notifications - plays alarm sound and vibrates like emergency alerts",
                LockscreenVisibility = NotificationVisibility.Public
            };

            channel.EnableVibration(true);
            channel.EnableLights(true);
            channel.LightColor = unchecked((int)0xFFFF0000); // Red
            channel.SetSound(alarmSound, audioAttributes);
            channel.SetBypassDnd(true); // Bypass Do Not Disturb

            var notificationManager = GetSystemService(NotificationService) as NotificationManager;
            notificationManager?.CreateNotificationChannel(channel);
#pragma warning restore CA1416
        }
    }
}
