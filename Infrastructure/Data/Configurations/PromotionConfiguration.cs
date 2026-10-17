using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class PromotionConfiguration : IEntityTypeConfiguration<Promotion>
{
    public void Configure(EntityTypeBuilder<Promotion> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(255).IsRequired();
        builder.Property(x => x.Description).HasColumnType("nvarchar(max)");
        builder.Property(x => x.Code).HasMaxLength(100);
        builder.HasIndex(x => x.Code).IsUnique().HasFilter("[Code] IS NOT NULL");
        builder.Property(x => x.IsActive).HasDefaultValue(true).HasSentinel(true);
        builder.Property(x => x.AppliesTo)
            .HasConversion(
                value => value.ToString().ToLowerInvariant(),
                value => Enum.Parse<PromotionAppliesTo>(value, true))
            .HasMaxLength(5).IsUnicode(false);
        builder.Property(x => x.DiscountType)
            .HasConversion(
                value => value.ToString().ToLowerInvariant(),
                value => Enum.Parse<DiscountType>(value, true))
            .HasMaxLength(10).IsUnicode(false);

        builder.HasOne(x => x.Restaurant).WithMany(x => x.Promotions)
            .HasForeignKey(x => x.RestaurantId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
