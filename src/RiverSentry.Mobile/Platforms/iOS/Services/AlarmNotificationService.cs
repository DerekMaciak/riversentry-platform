using AVFoundation;
using Foundation;

namespace RiverSentry.Mobile.Services;

public partial class AlarmNotificationService
{
    private AVAudioPlayer? _audioPlayer;
    private NSTimer? _loopTimer;

    // Only play the first N seconds of the flood warning sound, then restart
    private const double LoopDurationSeconds = 3.0;

    partial void StartAlarmAudio()
    {
        try
        {
            StopAlarmAudio();

            var url = NSBundle.MainBundle.GetUrlForResource("flood_warning", "mp3");
            if (url == null)
            {
                System.Diagnostics.Debug.WriteLine("AlarmNotificationService: flood_warning.mp3 not found in bundle");
                return;
            }

            _audioPlayer = AVAudioPlayer.FromUrl(url, out var error);
            if (_audioPlayer == null || error != null)
            {
                System.Diagnostics.Debug.WriteLine($"AlarmNotificationService: Audio error: {error?.LocalizedDescription}");
                return;
            }

            // Force playback through the loud speaker at max volume
            var session = AVAudioSession.SharedInstance();
            session.SetCategory(
                AVAudioSessionCategory.PlayAndRecord,
                AVAudioSessionCategoryOptions.DefaultToSpeaker | AVAudioSessionCategoryOptions.DuckOthers);
            session.OverrideOutputAudioPort(AVAudioSessionPortOverride.Speaker, out _);
            session.SetActive(true);

            _audioPlayer.NumberOfLoops = 0; // We handle looping manually
            _audioPlayer.Volume = 1.0f;
            _audioPlayer.Play();

            // Timer resets playback to the beginning every N seconds
            _loopTimer = NSTimer.CreateRepeatingScheduledTimer(LoopDurationSeconds, _ =>
            {
                if (_audioPlayer != null)
                {
                    _audioPlayer.CurrentTime = 0;
                    _audioPlayer.Play();
                }
            });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"AlarmNotificationService: StartAlarmAudio error: {ex.Message}");
        }
    }

    partial void StopAlarmAudio()
    {
        try
        {
            _loopTimer?.Invalidate();
            _loopTimer?.Dispose();
            _loopTimer = null;

            if (_audioPlayer != null)
            {
                _audioPlayer.Stop();
                _audioPlayer.Dispose();
                _audioPlayer = null;
            }

            AVAudioSession.SharedInstance().SetActive(false);
        }
        catch { }
    }
}
