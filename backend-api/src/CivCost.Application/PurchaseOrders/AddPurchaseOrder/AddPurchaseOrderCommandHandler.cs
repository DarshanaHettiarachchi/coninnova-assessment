using CivCost.Application.Abstractions;
using CivCost.Domain.Abstractions;
using CivCost.Domain.PurchaseOrders;
using CivCost.Domain.Suppliers;
using CivCost.Infrastructure.Services;

namespace CivCost.Application.PurchaseOrders.AddPurchaseOrder;
internal sealed class AddPurchaseOrderCommandHandler : ICommandHandler<AddPurchaseOrderCommand>
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPurchaseOrderNumberGenerator _purchaseOrderNumberGenerator;
    private readonly ISupplierRepository _supplierRepository;

    public AddPurchaseOrderCommandHandler(
        IPurchaseOrderRepository purchaseOrderRepository,
        IUnitOfWork unitOfWork,
        IPurchaseOrderNumberGenerator purchaseOrderNumberGenerator,
        ISupplierRepository supplierRepository)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _unitOfWork = unitOfWork;
        _purchaseOrderNumberGenerator = purchaseOrderNumberGenerator;
        _supplierRepository = supplierRepository;
    }

    public async Task<Result> Handle(AddPurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        var totalAmount = new Money(request.TotalAmount);

        Supplier? supplier = await _supplierRepository.GetByIdAsync(request.SupplierId, cancellationToken);

        if (supplier is null)
        {
            return Result.Failure(SupplierErros.NotFound);
        }

        string poNumber = await _purchaseOrderNumberGenerator.GenerateAsync(cancellationToken);

        var po = PurchaseOrder.Create(
           poNumber: poNumber,
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
