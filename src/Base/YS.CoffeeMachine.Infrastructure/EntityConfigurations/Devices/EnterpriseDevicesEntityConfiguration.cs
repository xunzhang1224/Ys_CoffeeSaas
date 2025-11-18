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
    /// 设备软件信息
    /// </summary>
    public class EnterpriseDevicesEntityConfiguration : IEntityTypeConfiguration<EnterpriseDevices>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<EnterpriseDevices> eb)
        {
            eb.ToTable("EnterpriseDevices");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.BelongingEnterpriseId).IsRequired();
            eb.Property(t => t.DeviceId).IsRequired();
            eb.Property(t => t.EnterpriseId).IsRequired();
            eb.Property(t => t.DeviceAllocationType).HasConversion<string>().HasMaxLength(30).IsRequired();
            eb.Property(t => t.RecyclingTime).IsRequired(false);
            eb.Property(t => t.AllocateTime).IsRequired(false);

            eb.HasOne(o => o.Device).WithMany(m => m.EnterpriseDevices).HasForeignKey(f => f.DeviceId);
            eb.HasOne(o => o.Enterprise).WithMany(m => m.EnterpriseDevices).HasForeignKey(f => f.EnterpriseId);

        }
    }
}
