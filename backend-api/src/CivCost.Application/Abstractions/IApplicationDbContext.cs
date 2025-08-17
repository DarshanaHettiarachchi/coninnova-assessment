using CivCost.Domain.PurchaseOrders;
using CivCost.Domain.Suppliers;
using Microsoft.EntityFrameworkCore;

namespace CivCost.Application.Abstractions;

public interface IApplicationDbContext
{
    public DbSet<Supplier> Suppliers { get; }
    public DbSet<PurchaseOrder> PurchaseOrders { get; }
}
