using Microsoft.AspNetCore.Mvc;
using RiverSentry.Application.Services;

namespace RiverSentry.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevicesController : ControllerBase
{
    private readonly DeviceService _deviceService;

    public DevicesController(DeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    /// <summary>
    /// Get all devices (anonymous access).
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var devices = await _deviceService.GetAllDevicesAsync(ct);
        return Ok(devices);
    }

    /// <summary>
    /// Get a specific device by ID (anonymous access).
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var device = await _deviceService.GetDeviceByIdAsync(id, ct);
        return device is null ? NotFound() : Ok(device);
    }

    /// <summary>
    /// Get latest status for a device (anonymous access).
    /// </summary>
    [HttpGet("{id:guid}/status")]
    public async Task<IActionResult> GetStatus(Guid id, CancellationToken ct)
    {
        var status = await _deviceService.GetLatestStatusAsync(id, ct);
        return status is null ? NotFound() : Ok(status);
    }

    /// <summary>
    /// Get map markers for all devices (anonymous access).
    /// </summary>
    [HttpGet("markers")]
    public async Task<IActionResult> GetMarkers(CancellationToken ct)
    {
        var markers = await _deviceService.GetMapMarkersAsync(ct);
        return Ok(markers);
    }
}
