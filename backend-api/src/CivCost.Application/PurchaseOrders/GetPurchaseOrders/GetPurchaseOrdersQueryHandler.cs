using CivCost.Application.Abstractions;
using CivCost.Application.Common;
using CivCost.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CivCost.Application.PurchaseOrders.GetPurchaseOrders;
internal sealed class GetPurchaseOrdersQueryHandler : IQueryHandler<GetPurchaseOrdersQuery, PaginatedResult<PurchaseOrderResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetPurchaseOrdersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<PaginatedResult<PurchaseOrderResponse>>> Handle(GetPurchaseOrdersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.PurchaseOrders
                .AsNoTracking()
                .AsQueryable();

        // Filter by SupplierId
        if (request.SupplierId.HasValue)
            query = query.Where(po => po.SupplierId == request.SupplierId.Value);

        // Filter by Status
        if (request.Status.HasValue)
            query = query.Where(po => po.Status == request.Status.Value);

        // Filter by date range
        if (request.FromDate.HasValue)
            query = query.Where(po => po.OrderDate >= request.FromDate.Value);

        if (request.ToDate.HasValue)
            query = query.Where(po => po.OrderDate <= request.ToDate.Value);

        // Sorting
        query = request.SortBy?.ToUpperInvariant() switch
        {
            "PONUMBER" => request.SortDirection == SortDirection.Asc
                ? query.OrderBy(po => po.PoNumber)
                : query.OrderByDescending(po => po.PoNumber),

            "ORDERDATE" => request.SortDirection == SortDirection.Asc
                ? query.OrderBy(po => po.OrderDate)
                : query.OrderByDescending(po => po.OrderDate),

            "TOTALAMOUNT" => request.SortDirection == SortDirection.Asc
                ? query.OrderBy(po => po.TotalAmount.Amount)
                : query.OrderByDescending(po => po.TotalAmount.Amount),

            _ => query.OrderBy(po => po.PoNumber)
        };

        var count = await query.CountAsync(cancellationToken);

        //Paging
        query = query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize);

        //This is more effiecent for this use case rather than using navigatin properties and includes
        //selects only requreid coloumns
        //And as per the current requirements and assumptions No need of a navigation property for Supplier
        //So DDD guide lines are respected
        var purchaseOrders = await query
            .Join(
                _context.Suppliers,
                po => po.SupplierId,
                s => s.Id,
                (po, s) => new PurchaseOrderResponse
                {
                    Id = po.Id,
                    PoNumber = po.PoNumber,
                    Description = po.Description,
                    orderDate = po.OrderDate,
                    Status = po.Status,
                    TotalAmount = po.TotalAmount,
                    Supplier = new SupplierResponse
                    {
                        Id = s.Id,
                        Name = s.Name
                    }
                }
            )
            .ToListAsync(cancellationToken);

        //round up partial pages 
        var totalPages = (count + request.PageSize - 1) / request.PageSize;

        var result = new PaginatedResult<PurchaseOrderResponse>(
              Items: purchaseOrders,
              CurrentPage: request.Page,
              PageSize: request.PageSize,
              TotalItems: count,
              TotalPages: totalPages
        );

        return result;

    }
}
