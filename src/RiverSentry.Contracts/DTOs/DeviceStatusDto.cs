using RiverSentry.Domain.Enums;

namespace RiverSentry.Contracts.DTOs;

/// <summary>
/// Device status snapshot for end-user display.
/// </summary>
public class DeviceStatusDto
{
    public Guid DeviceId { get; set; }
    public DeviceState State { get; set; }
    public double MainVolts { get; set; }
    public double BatteryVolts { get; set; }
    public bool WaterWitch1 { get; set; }
    public bool WaterWitch2 { get; set; }
    public bool WifiConnected { get; set; }
    public DateTime ReceivedAt { get; set; }

    public bool HasWaterDetection => WaterWitch1 || WaterWitch2;
}
