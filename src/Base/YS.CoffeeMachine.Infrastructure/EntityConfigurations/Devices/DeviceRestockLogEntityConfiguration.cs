using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Devices
{
    /// <summary>
    /// a
    /// </summary>
    public class DeviceRestockLogEntityConfiguration : IEntityTypeConfiguration<DeviceRestockLog>
    {
        /// <summary>
        /// b
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<DeviceRestockLog> builder)
        {
            builder.ToTable("DeviceRestockLog");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            builder.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            builder.HasMany(t => t.DeviceRestockLogSubs)
                   .WithOne(x => x.DeviceRestockLog)
                   .HasForeignKey(ur => ur.DeviceRestockLogId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();
        }
    }
}
