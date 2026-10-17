using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class SelectedOptionConfiguration : IEntityTypeConfiguration<SelectedOption>
{
    public void Configure(EntityTypeBuilder<SelectedOption> builder)
    {
        builder.HasIndex(x => new { x.CartItemId, x.OptionId }).IsUnique();

        builder.HasOne(x => x.CartItem).WithMany(x => x.SelectedOptions)
            .HasForeignKey(x => x.CartItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Option).WithMany(x => x.SelectedOptions)
            .HasForeignKey(x => x.OptionId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
