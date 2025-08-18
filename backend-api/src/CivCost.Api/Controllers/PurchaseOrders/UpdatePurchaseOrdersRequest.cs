using CivCost.Domain.PurchaseOrders;

namespace CivCost.Api.Controllers.PurchaseOrders;

public sealed record UpdatePurchaseOrderRequest(
    string Description,
    Guid SupplierId,
    DateOnly OrderDate,
    decimal TotalAmount,
    PurchaseOrderStatus Status
);
