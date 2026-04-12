using RiverSentry.Domain.Enums;

namespace RiverSentry.Contracts.DTOs;

/// <summary>
/// Alert event data for end-user display.
/// </summary>
public class AlertEventDto
{
    public Guid Id { get; set; }
    public Guid DeviceId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public string? FamilyName { get; set; }
    public AlarmType AlarmType { get; set; }
    public AlertSeverity Severity { get; set; }
    public string? Description { get; set; }
    public DateTime TriggeredAt { get; set; }
    public DateTime? AcknowledgedAt { get; set; }
    public string? AcknowledgedBy { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public bool IsActive { get; set; }

    /// <summary>Alert type as string for display</summary>
    public string AlertType => AlarmType.ToString();
}
