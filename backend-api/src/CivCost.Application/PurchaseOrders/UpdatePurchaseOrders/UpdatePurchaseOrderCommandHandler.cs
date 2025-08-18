using CivCost.Application.Abstractions;
using CivCost.Domain.Abstractions;
using CivCost.Domain.PurchaseOrders;

namespace CivCost.Application.PurchaseOrders.UpdatePurchaseOrders;
internal sealed class UpdatePurchaseOrderCommandHandler : ICommandHandler<UpdatePurchaseOrderCommand>
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdatePurchaseOrderCommandHandler(IPurchaseOrderRepository purchaseOrderRepository, IUnitOfWork unitOfWork)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdatePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        var po = await _purchaseOrderRepository.GetByIdAsync(request.Id, cancellationToken);

        if (po is null)
        {
            return Result.Failure(PurchaseOrderErrors.NotFound);
        }

        var totalAmount = new Money(request.TotalAmount);

        po.Update(
            description: request.Description,
            totalAmount: totalAmount,
            supplierId: request.SupplierId,
            orderDate: request.OrderDate
        );

        if (po.Status != request.Status)
        {
            po.UpdateStatus(request.Status);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();

    }
}
