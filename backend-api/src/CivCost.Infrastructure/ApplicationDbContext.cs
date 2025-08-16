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
             new
             {
                 Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                 Name = "Acme Corp",
                 Email = "info@acme.com",
                 Phone = "123-456-7890"
             },
              new
              {
                  Id = Guid.Parse("11111111-1111-1111-1111-111111111112"),
                  Name = "Tcme Corp",
                  Email = "info@Tcme.com",
                  Phone = "123-466-7890"
              },
              new
              {
                  Id = Guid.Parse("11111111-1111-1111-1111-111111111113"),
                  Name = "Rcme Corp",
                  Email = "info@Rcme.com",
                  Phone = "123-656-7890"
              }
        );

        base.OnModelCreating(modelBuilder);
    }

}
