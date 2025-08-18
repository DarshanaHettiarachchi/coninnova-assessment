namespace CivCost.Application.Suppliers.GetSuppliers;
public sealed record SupplierResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}
