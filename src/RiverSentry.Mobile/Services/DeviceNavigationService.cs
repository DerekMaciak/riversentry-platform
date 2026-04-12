using RiverSentry.Contracts.DTOs;

namespace RiverSentry.Mobile.Services;

/// <summary>
/// Simple service to pass device data between MAUI pages and Blazor components.
/// </summary>
public class DeviceNavigationService
{
    public DeviceDto? CurrentDevice { get; set; }
    public Action? CloseModal { get; set; }
}
