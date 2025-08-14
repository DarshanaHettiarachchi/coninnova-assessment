using CivCost.Domain.PurchaseOrders;
using CivCost.Domain.Suppliers;
using Microsoft.EntityFrameworkCore;

namespace CivCost.Infrastructure;

public sealed class ApplicationDbContext : DbContext
{
    public DbSet<Supplier> Suppliers { get; private set; }

    public DbSet<PurchaseOrder> PurchaseOrders { get; private set; }

    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

}
