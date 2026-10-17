using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.HasOne(x => x.Restaurant).WithOne(x => x.Menu)
            .HasForeignKey<Menu>(x => x.RestaurantId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
