using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.Order;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Order
{
    /// <summary>
    /// a
    /// </summary>
    public class OrderInfoEntityConfiguration : IEntityTypeConfiguration<OrderInfo>
    {
        /// <summary>
        /// a
        /// </summary>
        /// <param name="builder"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Configure(EntityTypeBuilder<OrderInfo> builder)
        {
            builder.ToTable("OrderInfo");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            builder.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            builder.HasMany(t => t.OrderDetails)
                   .WithOne(x => x.OrderInfo)
                   .HasForeignKey(ur => ur.OrderId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();
            builder.HasIndex(e => e.ThirdOrderId)
              .IsUnique()
              .HasFilter("[ThirdOrderId] IS NOT NULL");

            builder.HasIndex(e => e.ThirdOrderNo)
                  .IsUnique()
                  .HasFilter("[ThirdOrderNo] IS NOT NULL");
        }
    }
}
