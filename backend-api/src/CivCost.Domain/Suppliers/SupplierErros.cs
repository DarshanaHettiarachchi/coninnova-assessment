using CivCost.Domain.Abstractions;

namespace CivCost.Domain.Suppliers;
public static class SupplierErros
{
    public static readonly Error NotFound = new(
           "Supplier.NotFound",
           "The Supplier with the specified identifier was not found"
     );
}
