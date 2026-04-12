using RiverSentry.Domain.Enums;
using RiverSentry.UI.Shared.Services;

namespace RiverSentry.Mobile.Services;

/// <summary>
/// Mobile alarm audio playback using MAUI's built-in audio capabilities.
/// Uses the raw resources bundled with the app.
/// TODO: Replace with Plugin.Maui.Audio for more robust playback in Phase 3.
/// </summary>
public class MobileAlarmAudioService : IAlarmAudioService
{
    public bool IsPlaying { get; private set; }

    public async Task PlayAlarmAsync(AlarmType alarmType)
    {
        // TODO: Implement native audio playback
        // Phase 1: stub — will be implemented with Plugin.Maui.Audio
        // The sound files will be placed in Resources/Raw/
        // - flood_warning.mp3
        // - high_water_alarm.mp3
        IsPlaying = true;
        await Task.CompletedTask;
    }

    public async Task StopAsync()
    {
        IsPlaying = false;
        await Task.CompletedTask;
    }
}
