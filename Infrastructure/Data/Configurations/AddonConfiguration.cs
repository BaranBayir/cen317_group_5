using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class AddonConfiguration : IEntityTypeConfiguration<Addon>
{
    public void Configure(EntityTypeBuilder<Addon> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(255).IsRequired();
        builder.Property(x => x.PriceModifier).HasDefaultValue(0m);

        builder.HasOne(x => x.CategoryItem).WithMany(x => x.Addons)
            .HasForeignKey(x => x.CategoryItemId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
