using CivCost.Domain.Suppliers;

namespace CivCost.Infrastructure.Repositories;
internal sealed class SupplierRepository : Repository<Supplier>, ISupplierRepository
{
    public SupplierRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
