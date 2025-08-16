using CivCost.Domain.Abstractions;
using CivCost.Domain.PurchaseOrders;
using CivCost.Domain.Suppliers;
using Microsoft.EntityFrameworkCore;

namespace CivCost.Infrastructure;

public sealed class ApplicationDbContext : DbContext, IUnitOfWork
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

        modelBuilder.Entity<Supplier>().HasData(
            Supplier.Create(
               name: "Supplier one",
               email: "suplierone@email.com",
               phone: "07712345567"
            )
        );

        modelBuilder.Entity<Supplier>().HasData(
            Supplier.Create(
               name: "Supplier Two",
               email: "supliertwo@email.com",
               phone: "07712345568"
            )
        );

        modelBuilder.Entity<Supplier>().HasData(
            Supplier.Create(
               name: "Supplier Three",
               email: "suplierthree@email.com",
               phone: "07712345569"
            )
        );

        base.OnModelCreating(modelBuilder);
    }

}
