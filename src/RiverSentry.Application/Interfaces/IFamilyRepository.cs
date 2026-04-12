using RiverSentry.Domain.Entities;

namespace RiverSentry.Application.Interfaces;

public interface IFamilyRepository
{
    Task<IReadOnlyList<Family>> GetAllAsync(CancellationToken ct = default);
    Task<Family?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Family family, CancellationToken ct = default);
    Task UpdateAsync(Family family, CancellationToken ct = default);
}
