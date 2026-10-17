using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class OptionConfiguration : IEntityTypeConfiguration<Option>
{
    public void Configure(EntityTypeBuilder<Option> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(255).IsRequired();
        builder.Property(x => x.PriceModifier).HasDefaultValue(0m);

        builder.HasOne(x => x.OptionGroup).WithMany(x => x.Options)
            .HasForeignKey(x => x.OptionGroupId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
