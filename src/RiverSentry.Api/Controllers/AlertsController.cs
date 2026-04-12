using Microsoft.AspNetCore.Mvc;
using RiverSentry.Application.Services;

namespace RiverSentry.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlertsController : ControllerBase
{
    private readonly AlertService _alertService;

    public AlertsController(AlertService alertService)
    {
        _alertService = alertService;
    }

    /// <summary>
    /// Get recent alert events (anonymous access).
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetRecent([FromQuery] int count = 50, CancellationToken ct = default)
    {
        var alerts = await _alertService.GetRecentAlertsAsync(count, ct);
        return Ok(alerts);
    }

    /// <summary>
    /// Get currently active alerts (anonymous access).
    /// </summary>
    [HttpGet("active")]
    public async Task<IActionResult> GetActive(CancellationToken ct)
    {
        var alerts = await _alertService.GetActiveAlertsAsync(ct);
        return Ok(alerts);
    }
}
