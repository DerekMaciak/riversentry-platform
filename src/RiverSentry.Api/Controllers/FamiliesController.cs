using Microsoft.AspNetCore.Mvc;
using RiverSentry.Application.Services;
using RiverSentry.Contracts.DTOs;

namespace RiverSentry.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FamiliesController : ControllerBase
{
    private readonly FamilyService _familyService;

    public FamiliesController(FamilyService familyService)
    {
        _familyService = familyService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<DeviceFamilyDto>>> GetFamilies(CancellationToken ct)
    {
        var families = await _familyService.GetAllFamiliesAsync(ct);

        var dtos = families.Select(f => new DeviceFamilyDto
        {
            Id = f.Id,
            Name = f.Name,
            Description = f.Description,
            WebsiteUrl = f.WebsiteUrl,
            LogoUrl = f.LogoUrl,
            Address = f.Address,
            ContactEmail = f.ContactEmail,
            DeviceCount = f.Devices?.Count ?? 0,
            OnlineCount = 0, // RS-1B
            OfflineCount = 0 // RS-1B
        }).OrderBy(f => f.Name).ToList();

        return Ok(dtos);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DeviceFamilyDto>> GetFamily(Guid id, CancellationToken ct)
    {
        var family = await _familyService.GetFamilyByIdAsync(id, ct);

        if (family == null)
            return NotFound();

        return Ok(new DeviceFamilyDto
        {
            Id = family.Id,
            Name = family.Name,
            Description = family.Description,
            WebsiteUrl = family.WebsiteUrl,
            LogoUrl = family.LogoUrl,
            Address = family.Address,
            ContactEmail = family.ContactEmail,
            DeviceCount = family.Devices?.Count ?? 0,
            OnlineCount = 0,
            OfflineCount = 0
        });
    }
}
