using RiverSentry.Application.Interfaces;
using RiverSentry.Domain.Entities;

namespace RiverSentry.Application.Services;

public class FamilyService
{
    private readonly IFamilyRepository _familyRepo;

    public FamilyService(IFamilyRepository familyRepo)
    {
        _familyRepo = familyRepo;
    }

    public async Task<IReadOnlyList<Family>> GetAllFamiliesAsync(CancellationToken ct = default)
    {
        return await _familyRepo.GetAllAsync(ct);
    }

    public async Task<Family?> GetFamilyByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _familyRepo.GetByIdAsync(id, ct);
    }
}
