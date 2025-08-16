namespace CivCost.Infrastructure.Services;

public interface IPurchaseOrderNumberGenerator
{
    Task<string> GenerateAsync(CancellationToken cancellationToken = default);
}