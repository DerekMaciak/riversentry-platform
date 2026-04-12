using RiverSentry.Domain.Enums;

namespace RiverSentry.Contracts.DTOs;

/// <summary>
/// User's subscription to a device's alerts.
/// </summary>
public class SubscriptionDto
{
    public Guid Id { get; set; }
    public Guid DeviceId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public AlarmType[]? AlarmTypes { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
