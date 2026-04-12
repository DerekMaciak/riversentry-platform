namespace RiverSentry.Domain.Entities;

/// <summary>
/// A registered end-user of the River Sentry platform.
/// </summary>
public class AppUser
{
    public Guid Id { get; set; }

    /// <summary>External identity provider ID (e.g., Azure AD B2C object ID)</summary>
    public string ExternalId { get; set; } = string.Empty;

    /// <summary>User email address</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Display name</summary>
    public string? DisplayName { get; set; }

    /// <summary>When the user first registered</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>When the user last logged in</summary>
    public DateTime? LastLoginAt { get; set; }

    // Navigation properties
    public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    public ICollection<DeviceRegistration> DeviceRegistrations { get; set; } = new List<DeviceRegistration>();
}
