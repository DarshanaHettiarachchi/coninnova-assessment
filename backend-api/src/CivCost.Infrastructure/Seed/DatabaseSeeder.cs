namespace CivCost.Infrastructure.Seed;

using CivCost.Domain.PurchaseOrders;
using CivCost.Domain.Suppliers;
using Microsoft.EntityFrameworkCore;

internal sealed record SupplierSeedDto(Guid Id, string Name, string Email, string Phone);
internal sealed record PurchaseOrderSeedDto(
    Guid Id, string PoNumber,
    string Description,
    DateOnly OrderDate,
    PurchaseOrderStatus Status,
    decimal Amount,
    string Currency,
    Guid SupplierId
);


public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (!await context.Suppliers.AnyAsync())
            await SeedSuppliersAsync(context);

        if (!await context.PurchaseOrders.AnyAsync())
            await SeedPurchaseOrdersAsync(context);
    }

    private static async Task SeedSuppliersAsync(ApplicationDbContext context)
    {
        var supplierDtos = new[]
        {
            new SupplierSeedDto(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Acme Corp", "contact@acme.com", "+111-111-1111"),
            new SupplierSeedDto(Guid.Parse("22222222-2222-2222-2222-222222222222"), "Globex Ltd", "info@globex.com", "+222-222-2222"),
            new SupplierSeedDto(Guid.Parse("33333333-3333-3333-3333-333333333333"), "Initech", "support@initech.com", "+333-333-3333")
        };

        foreach (var dto in supplierDtos)
        {
            var supplier = Supplier.Create(dto.Name, dto.Email, dto.Phone);
            context.Suppliers.Add(supplier);
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedPurchaseOrdersAsync(ApplicationDbContext context)
    {
        var suppliers = await context.Suppliers.ToArrayAsync();
        var purchaseOrderDtos = new List<PurchaseOrderSeedDto>();

        for (int i = 1; i <= 20; i++)
        {
            var supplier = suppliers[i % suppliers.Length];
            purchaseOrderDtos.Add(new PurchaseOrderSeedDto(
                Guid.Parse($"aaaa{i:D4}-1111-1111-1111-111111111111"),
                $"PO-2025-{i:D5}",
                $"Purchase Order {i}",
                DateOnly.FromDateTime(DateTime.Today.AddDays(-i)),
                PurchaseOrderStatus.Draft,
                100 + (i * 10),
                "EUR",
                supplier.Id
            ));
        }

        foreach (var dto in purchaseOrderDtos)
        {
            var po = PurchaseOrder.Create(
                dto.PoNumber,
                dto.Description,
                dto.OrderDate,
                new Money(dto.Amount, dto.Currency),
                dto.Status,
                dto.SupplierId
            );
            context.PurchaseOrders.Add(po);
        }

        await context.SaveChangesAsync();
    }
}
