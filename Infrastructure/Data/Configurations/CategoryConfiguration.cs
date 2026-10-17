using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(255).IsRequired();

        builder.HasOne(x => x.Menu).WithMany(x => x.Categories)
            .HasForeignKey(x => x.MenuId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
