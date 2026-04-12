using RiverSentry.Domain.Enums;

namespace RiverSentry.Contracts.Requests;

/// <summary>
/// Status update sent by a device to the API.
/// </summary>
public class DeviceStatusUpdateRequest
{
    public DeviceState State { get; set; }
    public double MainVolts { get; set; }
    public double BatteryVolts { get; set; }
    public bool WaterWitch1 { get; set; }
    public bool WaterWitch2 { get; set; }
    public bool WifiConnected { get; set; }
    public bool NtpSync { get; set; }
}
