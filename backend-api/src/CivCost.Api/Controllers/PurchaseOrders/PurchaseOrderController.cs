using CivCost.Application.PurchaseOrders.AddPurchaseOrder;
using CivCost.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CivCost.Api.Controllers.PurchaseOrders;

[Route("api/[controller]")]
[ApiController]
public class PurchaseOrderController : ControllerBase
{

    private readonly ISender _sender;

    public PurchaseOrderController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> AddPurchaseOrder(AddPurchaseOrderRequest request, CancellationToken cancellationToken)
    {
        var command = new AddPurchaseOrderCommand(
            request.SupplierId,
            request.Description,
            request.OrderDate,
            request.TotalAmount
         );

        Result result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }

}
