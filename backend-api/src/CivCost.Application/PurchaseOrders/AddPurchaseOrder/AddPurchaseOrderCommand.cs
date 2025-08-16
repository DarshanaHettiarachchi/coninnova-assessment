using CivCost.Application.Abstractions;

namespace CivCost.Application.PurchaseOrders.AddPurchaseOrder;
public sealed record AddPurchaseOrderCommand(
    Guid SupplierId,
    string Description,
    DateOnly OrderDate,
    decimal TotalAmount
 ) : ICommand;
