using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class CourierConfiguration : IEntityTypeConfiguration<Courier>
{
    public void Configure(EntityTypeBuilder<Courier> builder)
    {
        builder.Property(x => x.IsAvailable).HasDefaultValue(true).HasSentinel(true);

        builder.HasOne<ApplicationUser>().WithOne()
            .HasForeignKey<Courier>(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
