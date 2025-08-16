using CivCost.Domain.Abstractions;

namespace CivCost.Domain.PurchaseOrders;
public static class PurchaseOrderErrors
{
    public static readonly Error NotApproved = new(
      "Booking.NotApproved",
      "Only Draft orders can be approved."
    );

    public static readonly Error CannotCancelNonDraftOrShipped = new(
        "PurchaseOrder.CannotCancel",
        "Only draft or approved POs can be cancelled"
    );

    public static readonly Error CannotUpdate = new(
        "PurchaseOrder.CannotUpdate",
        "Only draft  POs can be cancelled"
    );
}
