using RiverSentry.Domain.Enums;

namespace RiverSentry.Contracts.Requests;

/// <summary>
/// Request to create a maintenance record.
/// </summary>
public class CreateMaintenanceRequest
{
    public Guid DeviceId { get; set; }
    public ServiceType ServiceType { get; set; }
    public DateTime PerformedAt { get; set; }
    public string? PerformedBy { get; set; }
    public string? Notes { get; set; }
    public string? FirmwareVersionBefore { get; set; }
    public string? FirmwareVersionAfter { get; set; }
    public double? NewLatitude { get; set; }
    public double? NewLongitude { get; set; }
}
