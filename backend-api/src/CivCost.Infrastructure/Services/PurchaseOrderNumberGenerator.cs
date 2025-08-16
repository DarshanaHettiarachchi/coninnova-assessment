using System.Globalization;
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
        var connection = _dbContext.Database.GetDbConnection();

        // Ensure connection is open
        if (connection.State != System.Data.ConnectionState.Open)
            await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT NEXT VALUE FOR PurchaseOrderSeq";

        var result = await command.ExecuteScalarAsync(cancellationToken);

        var nextVal = Convert.ToInt32(result, CultureInfo.InvariantCulture);

        // Format: PO-2025-000123
        return $"PO-{DateTime.UtcNow.Year}-{nextVal:D6}";
    }

}
