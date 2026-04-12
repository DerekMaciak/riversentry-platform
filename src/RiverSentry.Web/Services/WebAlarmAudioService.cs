using Microsoft.JSInterop;
using RiverSentry.Domain.Enums;
using RiverSentry.UI.Shared.Services;

namespace RiverSentry.Web.Services;

/// <summary>
/// Web-based alarm audio playback using JavaScript Audio API.
/// </summary>
public class WebAlarmAudioService : IAlarmAudioService
{
    private readonly IJSRuntime _js;

    public WebAlarmAudioService(IJSRuntime js) => _js = js;

    public bool IsPlaying { get; private set; }

    public async Task PlayAlarmAsync(AlarmType alarmType)
    {
        var soundFile = alarmType switch
        {
            AlarmType.FloodWarning => "sounds/flood-warning.mp3",
            AlarmType.HighWaterAlarm => "sounds/high-water-alarm.mp3",
            _ => throw new ArgumentOutOfRangeException(nameof(alarmType))
        };

        await _js.InvokeVoidAsync("riverSentryAudio.play", soundFile);
        IsPlaying = true;
    }

    public async Task StopAsync()
    {
        await _js.InvokeVoidAsync("riverSentryAudio.stop");
        IsPlaying = false;
    }
}
