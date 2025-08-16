namespace CivCost.Domain.PurchaseOrders;
public interface IPurchaseOrderRepository
{
    Task<PurchaseOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    void Add(PurchaseOrder purchaseOrder);
}
