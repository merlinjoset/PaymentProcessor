using Assessment.Domain.Providers;

namespace Assessment.Application.Abstractions;

public interface IProviderRepository
{
    Task AddAsync(PaymentProvider provider, CancellationToken ct);
    Task UpdateAsync(PaymentProvider provider, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
    Task<PaymentProvider?> GetAsync(Guid id, CancellationToken ct);
    Task<List<PaymentProvider>> GetListAsync(CancellationToken ct);

}
