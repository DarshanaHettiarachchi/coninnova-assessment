using CivCost.Domain.Abstractions;

namespace CivCost.Domain.PurchaseOrders;

public class PurchaseOrder : Entity
{
    private PurchaseOrder(
        Guid id,
        string poNumber,
        string description,
        DateTime orderDate,
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
    }

    //For EFcore
    private PurchaseOrder() { }

    public string PoNumber { get; private set; }
    public string Description { get; private set; }
    public DateTime OrderDate { get; private set; }
    public Money TotalAmount { get; private set; }
    public PurchaseOrderStatus Status { get; private set; }
    public Guid SupplierId { get; private set; }

    public static PurchaseOrder Create(
        string poNumber,
        string description,
        DateTime orderDate,
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
}

public enum PurchaseOrderStatus
{
    Draft,
    Approved,
    Shipped,
    Completed,
    Cancelled
}
