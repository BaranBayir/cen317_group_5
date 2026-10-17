using Domain.Entities;
using Infrastructure.Data.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class OrderStatusHistoryConfiguration : IEntityTypeConfiguration<OrderStatusHistory>
{
    public void Configure(EntityTypeBuilder<OrderStatusHistory> builder)
    {
        builder.Property(x => x.Status)
            .HasConversion(OrderStatusConversion.Converter)
            .HasMaxLength(20).IsUnicode(false);
        builder.Property(x => x.ChangedAt).HasDefaultValueSql("SYSUTCDATETIME()");

        builder.HasOne(x => x.Order).WithMany(x => x.StatusHistory)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
