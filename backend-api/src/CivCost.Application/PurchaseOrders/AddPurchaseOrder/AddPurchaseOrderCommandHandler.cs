using CivCost.Application.Abstractions;
using CivCost.Domain.Abstractions;
using CivCost.Domain.PurchaseOrders;

namespace CivCost.Application.PurchaseOrders.AddPurchaseOrder;
internal sealed class AddPurchaseOrderCommandHandler : ICommandHandler<AddPurchaseOrderCommand>
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddPurchaseOrderCommandHandler(IPurchaseOrderRepository purchaseOrderRepository, IUnitOfWork unitOfWork)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AddPurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        var totalAmount = new Money(request.TotalAmount);

        var po = PurchaseOrder.Create(
           poNumber: "PO-2025-10",
           description: request.Description,
           orderDate: request.OrderDate,
           totalAmount: totalAmount,
           supplierId: request.SupplierId,
           status: PurchaseOrderStatus.Draft
        );

        _purchaseOrderRepository.Add(po);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
