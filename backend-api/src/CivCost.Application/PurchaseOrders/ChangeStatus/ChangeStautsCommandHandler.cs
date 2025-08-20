using CivCost.Application.Abstractions;
using CivCost.Domain.Abstractions;
using CivCost.Domain.PurchaseOrders;

namespace CivCost.Application.PurchaseOrders.ChangeStatus;
public class ChangeStautsCommandHandler : ICommandHandler<ChangeStatusCommand>
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeStautsCommandHandler(IPurchaseOrderRepository purchaseOrderRepository, IUnitOfWork unitOfWork)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ChangeStatusCommand request, CancellationToken cancellationToken)
    {
        var po = await _purchaseOrderRepository.GetByIdAsync(request.Id, cancellationToken);

        if (po is null)
        {
            return Result.Failure(PurchaseOrderErrors.NotFound);
        }

        var statusResult = po.UpdateStatus(request.Status);

        if (statusResult.IsFailure)
        {
            return statusResult;

        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
