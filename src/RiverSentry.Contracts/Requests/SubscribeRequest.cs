using RiverSentry.Domain.Enums;

namespace RiverSentry.Contracts.Requests;

/// <summary>
/// Request to subscribe to alerts from a device.
/// </summary>
public class SubscribeRequest
{
    public Guid DeviceId { get; set; }

    /// <summary>Which alarm types to subscribe to (null = all)</summary>
    public AlarmType[]? AlarmTypes { get; set; }
}
