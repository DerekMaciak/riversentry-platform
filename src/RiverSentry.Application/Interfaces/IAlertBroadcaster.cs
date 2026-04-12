using RiverSentry.Contracts.DTOs;

namespace RiverSentry.Application.Interfaces;

/// <summary>
/// Broadcasts alert events to connected clients in real time (SignalR).
/// </summary>
public interface IAlertBroadcaster
{
    Task BroadcastAlertAsync(AlertEventDto alert, CancellationToken ct = default);
    Task BroadcastDeviceStatusAsync(DeviceStatusDto status, CancellationToken ct = default);
}
