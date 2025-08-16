using Microsoft.EntityFrameworkCore;

namespace CivCost.Infrastructure.Services;

public class PurchaseOrderNumberGenerator : IPurchaseOrderNumberGenerator
{
    private readonly ApplicationDbContext _dbContext;

    public PurchaseOrderNumberGenerator(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> GenerateAsync(CancellationToken cancellationToken = default)
    {
        var nextVal = await _dbContext.Database
             .SqlQuery<int>($"SELECT NEXT VALUE FOR PurchaseOrderSeq")
             .SingleAsync(cancellationToken);

        // Format: PO-2025-000123
        return $"PO-{DateTime.UtcNow.Year}-{nextVal:D6}";
    }
}
