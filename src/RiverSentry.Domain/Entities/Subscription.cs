using RiverSentry.Domain.Enums;

namespace RiverSentry.Domain.Entities;

/// <summary>
/// A user's subscription to alerts from a specific device.
/// </summary>
public class Subscription
{
    public Guid Id { get; set; }

    /// <summary>The subscribed user</summary>
    public Guid UserId { get; set; }

    /// <summary>The device being subscribed to</summary>
    public Guid DeviceId { get; set; }

    /// <summary>Which alarm types the user wants notifications for (null = all)</summary>
    public AlarmType[]? AlarmTypes { get; set; }

    /// <summary>Whether the subscription is active</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>When the subscription was created</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public AppUser User { get; set; } = null!;
    public Device Device { get; set; } = null!;
}
