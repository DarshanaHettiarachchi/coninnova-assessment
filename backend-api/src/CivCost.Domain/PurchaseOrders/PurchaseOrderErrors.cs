using CivCost.Domain.Abstractions;

namespace CivCost.Domain.PurchaseOrders;
public static class PurchaseOrderErrors
{
    public static readonly Error NotApproved = new(
      "Booking.NotApproved",
      "Only Draft orders can be approved.");
}
