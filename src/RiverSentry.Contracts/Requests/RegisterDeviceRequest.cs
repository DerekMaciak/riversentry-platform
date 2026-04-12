using RiverSentry.Domain.Enums;

namespace RiverSentry.Contracts.Requests;

/// <summary>
/// Request to register a mobile device for push notifications.
/// </summary>
public class RegisterDeviceRequest
{
    public DevicePlatform Platform { get; set; }
    public string PushHandle { get; set; } = string.Empty;
}
