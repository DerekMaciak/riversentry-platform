using RiverSentry.Domain.Entities;

namespace RiverSentry.Application.Interfaces;

public interface IAlertRepository
{
    Task<IReadOnlyList<AlertEvent>> GetRecentAsync(int count = 50, CancellationToken ct = default);
    Task<IReadOnlyList<AlertEvent>> GetActiveAsync(CancellationToken ct = default);
    Task<AlertEvent?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(AlertEvent alert, CancellationToken ct = default);
    Task UpdateAsync(AlertEvent alert, CancellationToken ct = default);
}
