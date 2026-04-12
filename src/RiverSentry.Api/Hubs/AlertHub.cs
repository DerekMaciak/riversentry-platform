using Microsoft.AspNetCore.SignalR;

namespace RiverSentry.Api.Hubs;

/// <summary>
/// SignalR hub for real-time alert broadcasting.
/// Clients connect to receive live tower status updates and alert notifications.
/// </summary>
public class AlertHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}
