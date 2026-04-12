using Microsoft.AspNetCore.Mvc;
using RiverSentry.Application.Services;
using RiverSentry.Contracts.Requests;

namespace RiverSentry.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MaintenanceController : ControllerBase
{
    private readonly MaintenanceService _maintenanceService;

    public MaintenanceController(MaintenanceService maintenanceService)
    {
        _maintenanceService = maintenanceService;
    }

    /// <summary>
    /// Get recent maintenance records.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetRecent([FromQuery] int count = 50, CancellationToken ct = default)
    {
        var records = await _maintenanceService.GetRecentAsync(count, ct);
        return Ok(records);
    }

    /// <summary>
    /// Get a specific maintenance record by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var record = await _maintenanceService.GetByIdAsync(id, ct);
        return record is null ? NotFound() : Ok(record);
    }

    /// <summary>
    /// Get all maintenance records for a specific device.
    /// </summary>
    [HttpGet("device/{deviceId:guid}")]
    public async Task<IActionResult> GetByDevice(Guid deviceId, CancellationToken ct)
    {
        var records = await _maintenanceService.GetByDeviceAsync(deviceId, ct);
        return Ok(records);
    }

    /// <summary>
    /// Create a new maintenance record.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMaintenanceRequest request, CancellationToken ct)
    {
        try
        {
            var record = await _maintenanceService.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = record.Id }, record);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Delete a maintenance record.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _maintenanceService.DeleteAsync(id, ct);
        return NoContent();
    }
}
