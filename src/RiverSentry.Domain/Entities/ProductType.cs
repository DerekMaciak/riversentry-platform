namespace RiverSentry.Domain.Entities;

/// <summary>
/// A River Sentry product type (e.g., RS-1A, RS-1B).
/// </summary>
public class ProductType
{
    public int Id { get; set; }

    /// <summary>Product code (e.g., "RS-1A", "RS-1B")</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Display name (e.g., "River Sentry 1A")</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Product description</summary>
    public string? Description { get; set; }

    /// <summary>Whether this product type is currently active/sold</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Display order for UI lists</summary>
    public int DisplayOrder { get; set; }

    // Navigation property
    public ICollection<Device> Devices { get; set; } = new List<Device>();
}
