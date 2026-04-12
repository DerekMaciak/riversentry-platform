namespace RiverSentry.Domain.Enums;

/// <summary>
/// Operational state of a River Sentry device (end-user visible subset).
/// </summary>
public enum DeviceState
{
    Unknown = 0,

    /// <summary>Armed and actively monitoring</summary>
    Armed = 1,

    /// <summary>Water alarm triggered by local water detection</summary>
    AlarmWater = 2,

    /// <summary>Alarm triggered by an upstream device</summary>
    AlarmUpstream = 3,

    /// <summary>Silent alarm (no audible alert on device)</summary>
    AlarmSilent = 4,

    /// <summary>Test/drill alarm</summary>
    AlarmDrill = 5,

    /// <summary>Device is offline or unreachable</summary>
    Offline = 6
}
