using CivCost.Domain.PurchaseOrders;

namespace CivCost.Infrastructure.Repositories;
internal sealed class PurchaseOrderRepository : Repository<PurchaseOrder>, IPurchaseOrderRepository
{
    public PurchaseOrderRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
