using RiverSentry.Domain.Enums;

namespace RiverSentry.Contracts.DTOs;

/// <summary>
/// Maintenance record for display.
/// </summary>
public class MaintenanceRecordDto
{
    public Guid Id { get; set; }
    public Guid DeviceId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public ServiceType ServiceType { get; set; }
    public string ServiceTypeName { get; set; } = string.Empty;
    public DateTime PerformedAt { get; set; }
    public string? PerformedBy { get; set; }
    public string? Notes { get; set; }
    public string? FirmwareVersionBefore { get; set; }
    public string? FirmwareVersionAfter { get; set; }
    public double? NewLatitude { get; set; }
    public double? NewLongitude { get; set; }
    public DateTime CreatedAt { get; set; }
}
