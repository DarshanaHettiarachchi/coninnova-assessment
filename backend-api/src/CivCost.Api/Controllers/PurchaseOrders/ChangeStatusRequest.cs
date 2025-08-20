using CivCost.Domain.PurchaseOrders;

namespace CivCost.Api.Controllers.PurchaseOrders;
public record ChangeStatusRequest(PurchaseOrderStatus Status);
