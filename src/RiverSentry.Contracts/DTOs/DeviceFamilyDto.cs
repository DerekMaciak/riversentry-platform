namespace RiverSentry.Contracts.DTOs;

/// <summary>
/// Device family/watershed information.
/// </summary>
public class DeviceFamilyDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? LogoUrl { get; set; }
    public string? Address { get; set; }
    public string? ContactEmail { get; set; }
    public int DeviceCount { get; set; }
    public int OnlineCount { get; set; }
    public int OfflineCount { get; set; }
}
