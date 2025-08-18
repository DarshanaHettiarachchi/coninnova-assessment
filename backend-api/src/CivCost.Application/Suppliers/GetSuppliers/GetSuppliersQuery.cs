using CivCost.Application.Abstractions;

namespace CivCost.Application.Suppliers.GetSuppliers;

#pragma warning disable S2094 // Classes should not be empty
public sealed record GetSuppliersQuery : IQuery<IReadOnlyList<SupplierResponse>>;
#pragma warning restore S2094 // Classes should not be empty
