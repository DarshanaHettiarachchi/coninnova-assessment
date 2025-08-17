using CivCost.Application.Common;
using CivCost.Application.PurchaseOrders.AddPurchaseOrder;
using CivCost.Application.PurchaseOrders.GetPurchaseOrders;
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

    [HttpGet]
    public async Task<IActionResult> GetPurchaseOrders([FromQuery] GetPurchaseOrdersRequest request, CancellationToken cancellationToken)
    {
        var query = new GetPurchaseOrdersQuery(
            SupplierId: request.SupplierId,
            Status: request.Status,
            FromDate: request.FromDate,
            ToDate: request.ToDate,
            SortBy: request.SortBy,
            SortDirection: request.SortDirection,
            Page: request.Page,
            PageSize: request.PageSize
        );

        Result<PaginatedResult<PurchaseOrderResponse>> result = await _sender.Send(query, cancellationToken);
        return Ok(result.Value);
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
