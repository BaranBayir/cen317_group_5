using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class PromotionItemConfiguration : IEntityTypeConfiguration<PromotionItem>
{
    public void Configure(EntityTypeBuilder<PromotionItem> builder)
    {
        builder.HasKey(x => new { x.PromotionId, x.CategoryItemId });

        builder.HasOne(x => x.Promotion).WithMany(x => x.PromotionItems)
            .HasForeignKey(x => x.PromotionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.CategoryItem).WithMany(x => x.PromotionItems)
            .HasForeignKey(x => x.CategoryItemId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
