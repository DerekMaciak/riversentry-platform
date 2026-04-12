using Microsoft.AspNetCore.SignalR;
using RiverSentry.Api.Hubs;
using RiverSentry.Application.Interfaces;
using RiverSentry.Contracts.DTOs;

namespace RiverSentry.Api.Services;

/// <summary>
/// Broadcasts alerts and status updates to connected clients via SignalR.
/// </summary>
public class SignalRAlertBroadcaster : IAlertBroadcaster
{
    private readonly IHubContext<AlertHub> _hubContext;

    public SignalRAlertBroadcaster(IHubContext<AlertHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task BroadcastAlertAsync(AlertEventDto alert, CancellationToken ct = default)
    {
        await _hubContext.Clients.All.SendAsync("AlertRaised", alert, ct);
    }

    public async Task BroadcastDeviceStatusAsync(DeviceStatusDto status, CancellationToken ct = default)
    {
        await _hubContext.Clients.All.SendAsync("DeviceStatusUpdated", status, ct);
    }
}
