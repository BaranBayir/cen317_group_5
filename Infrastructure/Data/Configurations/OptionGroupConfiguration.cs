using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class OptionGroupConfiguration : IEntityTypeConfiguration<OptionGroup>
{
    public void Configure(EntityTypeBuilder<OptionGroup> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(255).IsRequired();
        builder.Property(x => x.MaxSelect).HasDefaultValue(1);

        builder.HasOne(x => x.CategoryItem).WithMany(x => x.OptionGroups)
            .HasForeignKey(x => x.CategoryItemId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
