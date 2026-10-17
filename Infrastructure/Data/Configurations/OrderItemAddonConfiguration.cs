using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class OrderItemAddonConfiguration : IEntityTypeConfiguration<OrderItemAddon>
{
    public void Configure(EntityTypeBuilder<OrderItemAddon> builder)
    {
        builder.Property(x => x.PriceModifier).HasDefaultValue(0m);
        builder.HasIndex(x => new { x.OrderItemId, x.AddonId }).IsUnique();

        builder.HasOne(x => x.OrderItem).WithMany(x => x.OrderItemAddons)
            .HasForeignKey(x => x.OrderItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Addon).WithMany(x => x.OrderItemAddons)
            .HasForeignKey(x => x.AddonId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
