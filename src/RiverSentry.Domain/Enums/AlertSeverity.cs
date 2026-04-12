namespace RiverSentry.Domain.Enums;

/// <summary>
/// Severity level of an alert event.
/// </summary>
public enum AlertSeverity
{
    /// <summary>Informational — no immediate action required</summary>
    Info = 0,

    /// <summary>Advisory — situation developing, be aware</summary>
    Advisory = 1,

    /// <summary>Warning — action may be required soon</summary>
    Warning = 2,

    /// <summary>Critical — immediate action required</summary>
    Critical = 3
}
