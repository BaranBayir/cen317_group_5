using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.Property(x => x.Quantity).HasDefaultValue(1);
        builder.Property(x => x.DiscountAmount).HasDefaultValue(0m);

        builder.HasOne(x => x.Order).WithMany(x => x.OrderItems)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.CategoryItem).WithMany(x => x.OrderItems)
            .HasForeignKey(x => x.CategoryItemId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Promotion).WithMany(x => x.OrderItems)
            .HasForeignKey(x => x.PromotionId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
