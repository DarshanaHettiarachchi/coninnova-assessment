using CivCost.Application.PurchaseOrders.GetPurchaseOrders;
using CivCost.Domain.PurchaseOrders;

namespace CivCost.Api.Controllers.PurchaseOrders;

public record GetPurchaseOrdersRequest(
    Guid? SupplierId,
    PurchaseOrderStatus? Status,
    DateOnly? FromDate,
    DateOnly? ToDate,
    string? SortBy,
    SortDirection SortDirection = SortDirection.Asc,
    int Page = 1,
    int PageSize = 10
);
