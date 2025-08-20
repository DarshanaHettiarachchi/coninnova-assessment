using CivCost.Application.Common;
using CivCost.Application.PurchaseOrders.AddPurchaseOrder;
using CivCost.Application.PurchaseOrders.ChangeStatus;
using CivCost.Application.PurchaseOrders.GetPurchaseOrders;
using CivCost.Application.PurchaseOrders.UpdatePurchaseOrders;
using CivCost.Domain.Abstractions;
using CivCost.Domain.PurchaseOrders;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CivCost.Api.Controllers.PurchaseOrders;

[Route("api/purchase-orders")]
[ApiController]
public class PurchaseOrdersController : ControllerBase
{

    private readonly ISender _sender;

    public PurchaseOrdersController(ISender sender)
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

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePurchaseOrder(Guid id, UpdatePurchaseOrderRequest request, CancellationToken cancellationToken)
    {
        var update = new UpdatePurchaseOrderCommand(
            Id: id,
            Description: request.Description,
            SupplierId: request.SupplierId,
            OrderDate: request.OrderDate,
            TotalAmount: request.TotalAmount,
            Status: request.Status
        );

        Result result = await _sender.Send(update, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> ChangeStatus(Guid id, PurchaseOrderStatus status, CancellationToken cancellationToken)
    {
        var update = new ChangeStatusCommand(
            Id: id,
            Status: status
        );

        Result result = await _sender.Send(update, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }

}
