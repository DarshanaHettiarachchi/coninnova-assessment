using CivCost.Domain.Abstractions;

namespace CivCost.Domain.PurchaseOrders;
public static class PurchaseOrderErrors
{
    public static readonly Error NotApproved = new(
          "PurchaseOrder.NotApproved",
          "Only draft purchase orders can be approved."
    );

    public static readonly Error CannotCancelNonDraftOrShipped = new(
        "PurchaseOrder.CannotCancel",
        "Only draft or approved purchase orders can be cancelled."
    );

    public static readonly Error AlreadyCancelled = new(
        "PurchaseOrder.AlreadyCancelled",
        "The purchase order is already cancelled."
    );

    public static readonly Error CannotUpdate = new(
        "PurchaseOrder.CannotUpdate",
        "Only draft purchase orders can be updated."
    );

    public static readonly Error CannotShip = new(
        "PurchaseOrder.CannotShip",
        "Only approved purchase orders can be shipped."
    );

    public static readonly Error CannotComplete = new(
        "PurchaseOrder.CannotComplete",
        "Only shipped purchase orders can be completed."
    );

    public static readonly Error CannotRevertToDraft = new(
        "PurchaseOrder.CannotRevertToDraft",
        "Only approved purchase orders can be reverted to draft."
    );

    public static readonly Error NotFound = new(
       "PurchaseOrder.NotFound",
       "The purchase order with the specified id not found."
   );
}
