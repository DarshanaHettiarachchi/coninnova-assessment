using CivCost.Application.Abstractions;
using CivCost.Domain.PurchaseOrders;

namespace CivCost.Application.PurchaseOrders.ChangeStatus;

public sealed record ChangeStatusCommand(Guid Id, PurchaseOrderStatus Status) : ICommand;
