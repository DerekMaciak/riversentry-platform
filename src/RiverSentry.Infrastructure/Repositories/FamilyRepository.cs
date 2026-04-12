using Microsoft.EntityFrameworkCore;
using RiverSentry.Application.Interfaces;
using RiverSentry.Domain.Entities;
using RiverSentry.Infrastructure.Data;

namespace RiverSentry.Infrastructure.Repositories;

public class FamilyRepository : IFamilyRepository
{
    private readonly RiverSentryDbContext _db;

    public FamilyRepository(RiverSentryDbContext db) => _db = db;

    public async Task<IReadOnlyList<Family>> GetAllAsync(CancellationToken ct = default)
        => await _db.Families
            .AsNoTracking()
            .Include(f => f.Devices)
            .OrderBy(f => f.Name)
            .ToListAsync(ct);

    public async Task<Family?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _db.Families
            .Include(f => f.Devices)
            .FirstOrDefaultAsync(f => f.Id == id, ct);

    public async Task AddAsync(Family family, CancellationToken ct = default)
    {
        _db.Families.Add(family);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Family family, CancellationToken ct = default)
    {
        _db.Families.Update(family);
        await _db.SaveChangesAsync(ct);
    }
}
