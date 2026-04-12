using RiverSentry.Domain.Enums;

namespace RiverSentry.Domain.Entities;

/// <summary>
/// An alert event raised by a device.
/// </summary>
public class AlertEvent
{
    public Guid Id { get; set; }

    /// <summary>The device that raised this alert</summary>
    public Guid DeviceId { get; set; }

    /// <summary>Type of alarm</summary>
    public AlarmType AlarmType { get; set; }

    /// <summary>Severity level</summary>
    public AlertSeverity Severity { get; set; }

    /// <summary>Human-readable description</summary>
    public string? Description { get; set; }

    /// <summary>When the alert was triggered</summary>
    public DateTime TriggeredAt { get; set; } = DateTime.UtcNow;

    /// <summary>When the alert was resolved (null if still active)</summary>
    public DateTime? ResolvedAt { get; set; }

    /// <summary>Whether the alert is currently active</summary>
    public bool IsActive => ResolvedAt == null;

    // Navigation property
    public Device Device { get; set; } = null!;
}
