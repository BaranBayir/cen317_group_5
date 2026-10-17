using Domain.Entities;
using Domain.Enums;
using Infrastructure.Identity;
using Infrastructure.Data.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(x => x.Status)
            .HasConversion(OrderStatusConversion.Converter)
            .HasMaxLength(20).IsUnicode(false)
            .HasDefaultValue(OrderStatus.Pending);
        builder.Property(x => x.DeliveryAddress).HasColumnType("nvarchar(max)").IsRequired();

        builder.HasOne<ApplicationUser>().WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Restaurant).WithMany(x => x.Orders)
            .HasForeignKey(x => x.RestaurantId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Courier).WithMany(x => x.Orders)
            .HasForeignKey(x => x.CourierId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Promotion).WithMany(x => x.Orders)
            .HasForeignKey(x => x.PromotionId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
