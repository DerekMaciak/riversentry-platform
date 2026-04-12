namespace RiverSentry.Domain.Entities;

/// <summary>
/// A family/group of devices at a location or owned by a customer.
/// </summary>
public class Family
{
    public Guid Id { get; set; }

    /// <summary>Family name (e.g., "Camp La Junta")</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Description of the family/location</summary>
    public string? Description { get; set; }

    /// <summary>Primary contact email for this family</summary>
    public string? ContactEmail { get; set; }

    /// <summary>Primary contact phone</summary>
    public string? ContactPhone { get; set; }

    /// <summary>Physical address or location description</summary>
    public string? Address { get; set; }

    /// <summary>Website URL for this location</summary>
    public string? WebsiteUrl { get; set; }

    /// <summary>Logo image URL for this location</summary>
    public string? LogoUrl { get; set; }

    /// <summary>When this family was created</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Whether this family is active</summary>
    public bool IsActive { get; set; } = true;

    // Navigation property
    public ICollection<Device> Devices { get; set; } = new List<Device>();
}
