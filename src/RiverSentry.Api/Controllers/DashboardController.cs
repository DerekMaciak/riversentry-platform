using Microsoft.AspNetCore.Mvc;
using RiverSentry.Application.Services;
using RiverSentry.Contracts.DTOs;

namespace RiverSentry.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly DeviceService _deviceService;
    private readonly FamilyService _familyService;

    public DashboardController(DeviceService deviceService, FamilyService familyService)
    {
        _deviceService = deviceService;
        _familyService = familyService;
    }

    [HttpGet("stats")]
    public async Task<ActionResult<DashboardStats>> GetStats(CancellationToken ct)
    {
        var devices = await _deviceService.GetAllDevicesAsync(ct);
        var families = await _familyService.GetAllFamiliesAsync(ct);

        return Ok(new DashboardStats
        {
            TotalDevices = devices.Count,
            TotalFamilies = families.Count,
            OnlineDevices = 0, // RS-1B
            OfflineDevices = 0, // RS-1B
            ActiveAlerts = 0 // RS-1B
        });
    }
}
