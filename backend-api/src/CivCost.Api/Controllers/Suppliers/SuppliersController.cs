using CivCost.Application.Suppliers.GetSuppliers;
using CivCost.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CivCost.Api.Controllers.Suppliers;
[Route("api/suppliers")]
[ApiController]
public class SuppliersController : ControllerBase
{
    private readonly ISender _sender;
    public SuppliersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetSuppliers(CancellationToken cancellationToken)
    {
        var query = new GetSuppliersQuery();

        Result<IReadOnlyList<SupplierResponse>> result =
            await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }
}
