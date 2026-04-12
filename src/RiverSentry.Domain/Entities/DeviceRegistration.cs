using RiverSentry.Domain.Enums;

namespace RiverSentry.Domain.Entities;

/// <summary>
/// A mobile device registered for push notifications.
/// </summary>
public class DeviceRegistration
{
    public Guid Id { get; set; }

    /// <summary>The user who owns this device</summary>
    public Guid UserId { get; set; }

    /// <summary>Mobile platform (Android/iOS)</summary>
    public DevicePlatform Platform { get; set; }

    /// <summary>Push notification handle (FCM token or APNs device token)</summary>
    public string PushHandle { get; set; } = string.Empty;

    /// <summary>When the device was registered</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>When the device last checked in</summary>
    public DateTime? LastActiveAt { get; set; }

    // Navigation property
    public AppUser User { get; set; } = null!;
}
