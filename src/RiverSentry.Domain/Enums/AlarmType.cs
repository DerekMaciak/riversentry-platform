namespace RiverSentry.Domain.Enums;

/// <summary>
/// Types of alarms that can be triggered and heard by end users.
/// </summary>
public enum AlarmType
{
    /// <summary>Flood warning — advisory level</summary>
    FloodWarning = 1,

    /// <summary>High water alarm — critical level</summary>
    HighWaterAlarm = 2
}
