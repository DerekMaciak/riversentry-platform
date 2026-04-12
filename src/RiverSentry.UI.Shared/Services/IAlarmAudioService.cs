using RiverSentry.Domain.Enums;

namespace RiverSentry.UI.Shared.Services;

/// <summary>
/// Plays alarm sounds. Implemented differently on web (JS audio) vs mobile (native audio).
/// </summary>
public interface IAlarmAudioService
{
    Task PlayAlarmAsync(AlarmType alarmType);
    Task StopAsync();
    bool IsPlaying { get; }
}
