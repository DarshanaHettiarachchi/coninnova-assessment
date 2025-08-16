namespace CivCost.Api.Controllers.PurchaseOrders;

public sealed record AddPurchaseOrderRequest(
    Guid SupplierId,
    string Description,
    DateOnly OrderDate,
    decimal TotalAmount
);
