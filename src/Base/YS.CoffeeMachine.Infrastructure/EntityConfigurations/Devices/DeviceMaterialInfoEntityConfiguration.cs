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
    /// 物料表配置
    /// </summary>
    public class DeviceMaterialInfoEntityConfiguration : IEntityTypeConfiguration<DeviceMaterialInfo>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<DeviceMaterialInfo> eb)
        {
            eb.ToTable("DeviceMaterialInfo");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            //eb.HasKey(t => new { t.DeviceBaseId, t.Type, t.Index });
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.HasIndex(t => new { t.DeviceBaseId, t.Type, t.Index })
        .IsUnique();
        }
    }
}
