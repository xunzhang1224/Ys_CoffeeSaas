using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Log;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.AggregatesModel.Strategy;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Strategy
{
    /// <summary>
    /// 设备度量日志
    /// </summary>
    public class DeviceMetricsLogEntityConfiguration : IEntityTypeConfiguration<DeviceMetricsLog>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<DeviceMetricsLog> eb)
        {
            eb.ToTable("devicemetricslog");
            eb.HasKey(e => new { e.DeviceId, e.Timestamp });
            eb.Property(e => e.Timestamp)
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("(now() AT TIME ZONE 'UTC')") // 默认UTC时间
            .HasConversion(
                // 写入时：强制剥除时区信息，纯UTC值写入
                v => new DateTime(v.Ticks, DateTimeKind.Unspecified),

                // 读取时：强制标记为UTC
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );
            eb.HasIndex(e => new { e.DeviceId, e.Timestamp }).IsUnique();
        }
    }
}
