using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class SelectedAddonConfiguration : IEntityTypeConfiguration<SelectedAddon>
{
    public void Configure(EntityTypeBuilder<SelectedAddon> builder)
    {
        builder.HasIndex(x => new { x.CartItemId, x.AddonId }).IsUnique();

        builder.HasOne(x => x.CartItem).WithMany(x => x.SelectedAddons)
            .HasForeignKey(x => x.CartItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Addon).WithMany(x => x.SelectedAddons)
            .HasForeignKey(x => x.AddonId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
