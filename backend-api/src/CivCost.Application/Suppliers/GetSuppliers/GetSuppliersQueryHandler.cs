using CivCost.Application.Abstractions;
using CivCost.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CivCost.Application.Suppliers.GetSuppliers;
internal sealed class GetSuppliersQueryHandler : IQueryHandler<GetSuppliersQuery, IReadOnlyList<SupplierResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetSuppliersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IReadOnlyList<SupplierResponse>>> Handle(GetSuppliersQuery request, CancellationToken cancellationToken)
    {
        var suppliers = await _context.Suppliers.Select(s => new SupplierResponse
        {
            Id = s.Id,
            Name = s.Name,
        }).ToListAsync(cancellationToken);

        return suppliers;
    }
}
