using CivCost.Domain.Abstractions;

namespace CivCost.Domain.PurchaseOrders;

public class PurchaseOrder : Entity
{
    private PurchaseOrder(
        Guid id,
        string poNumber,
        string description,
        DateOnly orderDate,
        Money totalAmount,
        PurchaseOrderStatus status,
        Guid supplierId

    ) : base(id)
    {
        PoNumber = poNumber;
        Description = description;
        OrderDate = orderDate;
        TotalAmount = totalAmount;
        Status = status;
        SupplierId = supplierId;
    }

    //For EFcore
    private PurchaseOrder() { }

    public string PoNumber { get; private set; }
    public string Description { get; private set; }
    public DateOnly OrderDate { get; private set; }
    public Money TotalAmount { get; private set; }
    public PurchaseOrderStatus Status { get; private set; }
    public Guid SupplierId { get; private set; }

    public static PurchaseOrder Create(
        string poNumber,
        string description,
        DateOnly orderDate,
        Money totalAmount,
        PurchaseOrderStatus status,
        Guid supplierId
    )
    {

        return new PurchaseOrder(
            Guid.NewGuid(),
            poNumber,
            description,
            orderDate,
            totalAmount,
            status,
            supplierId
        );
    }

    /// <summary>
    /// Approves the purchase order if it is in <see cref="PurchaseOrderStatus.Draft"/> status.  
    /// </summary>
    /// <returns>
    /// A <see cref="Result"/> indicating success if the order was approved,  
    /// or failure with a corresponding error if approval is not allowed.
    /// </returns>
    public Result Approve()
    {
        if (Status != PurchaseOrderStatus.Draft)
            return Result.Failure(PurchaseOrderErrors.NotApproved);

        Status = PurchaseOrderStatus.Approved;
        return Result.Success();
    }

    public Result Ship()
    {
        if (Status != PurchaseOrderStatus.Approved)
            return Result.Failure(PurchaseOrderErrors.CannotShip);

        Status = PurchaseOrderStatus.Shipped;
        return Result.Success();
    }

    public Result Complete()
    {
        if (Status != PurchaseOrderStatus.Shipped)
            return Result.Failure(PurchaseOrderErrors.CannotComplete);

        Status = PurchaseOrderStatus.Completed;
        return Result.Success();
    }

    public Result Cancel()
    {
        if (Status == PurchaseOrderStatus.Shipped || Status == PurchaseOrderStatus.Completed)
            return Result.Failure(PurchaseOrderErrors.CannotCancelNonDraftOrShipped);

        if (Status == PurchaseOrderStatus.Cancelled)
            return Result.Failure(PurchaseOrderErrors.AlreadyCancelled);

        Status = PurchaseOrderStatus.Cancelled;
        return Result.Success();
    }

    public Result RevertToDraft()
    {
        if (Status != PurchaseOrderStatus.Approved)
            return Result.Failure(PurchaseOrderErrors.CannotRevertToDraft);

        Status = PurchaseOrderStatus.Draft;
        return Result.Success();
    }
    public Result Update(string description, Money totalAmount, DateOnly orderDate)
    {
        if (Status != PurchaseOrderStatus.Draft)
            return Result.Failure(PurchaseOrderErrors.CannotUpdate);

        Description = description;
        OrderDate = orderDate;
        TotalAmount = totalAmount;

        return Result.Success();
    }
    public Result UpdateStatus(PurchaseOrderStatus newStatus)
    {
        return newStatus switch
        {
            PurchaseOrderStatus.Approved => Approve(),
            PurchaseOrderStatus.Shipped => Ship(),
            PurchaseOrderStatus.Completed => Complete(),
            PurchaseOrderStatus.Cancelled => Cancel(),
            PurchaseOrderStatus.Draft => RevertToDraft(),
            _ => Result.Failure(new Error(
                "PurchaseOrder.InvalidStatusTransition",
                $"Transition to status '{newStatus}' is not allowed."
            ))
        };
    }
}

public enum PurchaseOrderStatus
{
    Draft,
    Approved,
    Shipped,
    Completed,
    Cancelled
}
