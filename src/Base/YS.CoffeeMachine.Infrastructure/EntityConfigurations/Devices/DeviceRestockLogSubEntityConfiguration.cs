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
    public class DeviceRestockLogSubEntityConfiguration : IEntityTypeConfiguration<DeviceRestockLogSub>
    {
        /// <summary>
        /// b
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<DeviceRestockLogSub> builder)
        {
            builder.ToTable("DeviceRestockLogSub");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
        }
    }
}
