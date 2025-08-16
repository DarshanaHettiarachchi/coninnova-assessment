using CivCost.Domain.PurchaseOrders;
using CivCost.Domain.Suppliers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CivCost.Infrastructure.Configurations;
internal sealed class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
{
    public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
    {
        builder.ToTable("PurchaseOrders");

        builder.HasKey(po => po.Id);

        builder.Property(u => u.PoNumber)
               .HasMaxLength(20)
               .IsUnicode(false);

        builder.HasIndex(po => po.PoNumber).IsUnique();

        builder.OwnsOne(po => po.TotalAmount, money =>
        {
            money.Property(m => m.Amount).HasPrecision(18, 2);
            money.Property(m => m.Currency).HasMaxLength(3);
        });

        builder.HasOne<Supplier>()
           .WithMany()
           .HasForeignKey(po => po.SupplierId);
    }
}
