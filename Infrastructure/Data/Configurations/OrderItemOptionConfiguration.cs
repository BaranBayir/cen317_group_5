using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class OrderItemOptionConfiguration : IEntityTypeConfiguration<OrderItemOption>
{
    public void Configure(EntityTypeBuilder<OrderItemOption> builder)
    {
        builder.Property(x => x.PriceModifier).HasDefaultValue(0m);
        builder.HasIndex(x => new { x.OrderItemId, x.OptionId }).IsUnique();

        builder.HasOne(x => x.OrderItem).WithMany(x => x.OrderItemOptions)
            .HasForeignKey(x => x.OrderItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Option).WithMany(x => x.OrderItemOptions)
            .HasForeignKey(x => x.OptionId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
