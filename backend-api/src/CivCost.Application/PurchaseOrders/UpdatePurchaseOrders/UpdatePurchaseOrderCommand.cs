using CivCost.Application.Abstractions;
using CivCost.Domain.PurchaseOrders;

namespace CivCost.Application.PurchaseOrders.UpdatePurchaseOrders;

public sealed record UpdatePurchaseOrderCommand(
    Guid Id,
    string Description,
    Guid SupplierId,
    DateOnly OrderDate,
    decimal TotalAmount,
    PurchaseOrderStatus Status
) : ICommand;
