using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Devices
{
    /// <summary>
    /// 111
    /// </summary>
    public class DeviceFlushComponentsEntityConfiguration : IEntityTypeConfiguration<DeviceFlushComponents>
    {
        /// <summary>
        /// 1
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<DeviceFlushComponents> builder)
        {
            builder.ToTable("DeviceFlushComponents");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            builder.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            builder.Property(t => t.Name).IsRequired(false);
        }
    }
}