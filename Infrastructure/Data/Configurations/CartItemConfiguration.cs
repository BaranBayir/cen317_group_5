using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.Property(x => x.Quantity).HasDefaultValue(1);

        builder.HasOne(x => x.Cart).WithMany(x => x.CartItems)
            .HasForeignKey(x => x.CartId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.CategoryItem).WithMany(x => x.CartItems)
            .HasForeignKey(x => x.CategoryItemId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
