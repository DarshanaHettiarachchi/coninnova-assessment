using CivCost.Domain.PurchaseOrders;
using FluentAssertions;

namespace CivCost.Domain.UnitTests.PurchaseOrders;
public class PurchaseOrderTests
{
    [Fact]
    public void Approve_Should_ReturnSuccess_WhenStatusIsDraft()
    {
        var po = PurchaseOrder.Create(
            poNumber: "PO-001",
            description: "Test PO",
            orderDate: DateTime.UtcNow,
            totalAmount: new Money(100m),
            status: PurchaseOrderStatus.Draft,
            supplierId: Guid.NewGuid()
        );

        var result = po.Approve();

        result.IsSuccess.Should().BeTrue();
        po.Status.Should().Be(PurchaseOrderStatus.Approved);
    }


    [Theory]
    [InlineData(PurchaseOrderStatus.Approved)]
    [InlineData(PurchaseOrderStatus.Cancelled)]
    [InlineData(PurchaseOrderStatus.Shipped)]
    [InlineData(PurchaseOrderStatus.Completed)]
    public void Approve_Should_ReturnFailure_ForNonDraftStatuses(PurchaseOrderStatus status)
    {
        var po = PurchaseOrder.Create(
            poNumber: "PO-002",
            description: "Test PO",
            orderDate: DateTime.UtcNow,
            totalAmount: new Money(100m),
            status: status,
            supplierId: Guid.NewGuid()
        );

        var result = po.Approve();

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(PurchaseOrderErrors.NotApproved);
        po.Status.Should().Be(status);
    }

    [Theory]
    [InlineData(PurchaseOrderStatus.Draft)]
    [InlineData(PurchaseOrderStatus.Approved)]
    public void Cancel_Should_ReturnSuccess_ForDraftOrApprovedStatuses(PurchaseOrderStatus status)
    {
        // Arrange
        var po = PurchaseOrder.Create(
            poNumber: "PO-001",
            description: "Test PO",
            orderDate: DateTime.UtcNow,
            totalAmount: new Money(100m),
            status: status,
            supplierId: Guid.NewGuid()
        );

        // Act
        var result = po.Cancel();

        // Assert
        result.IsSuccess.Should().BeTrue();
        po.Status.Should().Be(PurchaseOrderStatus.Cancelled);
    }

    [Theory]
    [InlineData(PurchaseOrderStatus.Shipped)]
    [InlineData(PurchaseOrderStatus.Completed)]
    [InlineData(PurchaseOrderStatus.Cancelled)]
    public void Cancel_Should_ReturnFailure_ForNonDraftOrApprovedStatuses(PurchaseOrderStatus status)
    {
        // Arrange
        var po = PurchaseOrder.Create(
            poNumber: "PO-002",
            description: "Test PO",
            orderDate: DateTime.UtcNow,
            totalAmount: new Money(100m),
            status: status,
            supplierId: Guid.NewGuid()
        );

        // Act
        var result = po.Cancel();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(PurchaseOrderErrors.CannotCancelNonDraftOrShipped);
        po.Status.Should().Be(status); // Status remains unchanged
    }

}
