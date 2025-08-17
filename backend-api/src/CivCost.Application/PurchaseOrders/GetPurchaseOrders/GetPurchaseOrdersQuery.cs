using CivCost.Application.Abstractions;
using CivCost.Domain.PurchaseOrders;

namespace CivCost.Application.PurchaseOrders.GetPurchaseOrders;

public enum SortDirection
{
    Asc,
    Desc
}

public sealed record GetPurchaseOrdersQuery(
    Guid? SupplierId,
    PurchaseOrderStatus? Status,
    DateOnly? FromDate,
    DateOnly? ToDate,
    string? SortBy,
    SortDirection SortDirection = SortDirection.Asc,
    int Page = 1,
    int PageSize = 10
) : IQuery<IReadOnlyList<PurchaseOrderResponse>>;
