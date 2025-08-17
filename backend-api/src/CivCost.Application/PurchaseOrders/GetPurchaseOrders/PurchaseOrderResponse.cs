using CivCost.Domain.PurchaseOrders;

namespace CivCost.Application.PurchaseOrders.GetPurchaseOrders;
public sealed record PurchaseOrderResponse
{
    public required Guid Id { get; init; }
    public required string PoNumber { get; init; }
    public required string Description { get; init; }
    public required PurchaseOrderStatus Status { get; init; }
    public required DateOnly orderDate { get; init; }
    public required Money TotalAmount { get; init; }
    public required SupplierResponse Supplier { get; init; }
}

public sealed record SupplierResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}
