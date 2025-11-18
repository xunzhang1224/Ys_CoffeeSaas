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
    /// 设备服务商
    /// </summary>
    public class DeviceServiceProvidersEntityConfiguration : IEntityTypeConfiguration<DeviceServiceProviders>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<DeviceServiceProviders> eb)
        {
            eb.ToTable("DeviceServiceProviders");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.DeviceInfoId).IsRequired();
            eb.Property(t => t.ServiceProviderInfoId).IsRequired();

            //设备一对多服务商
            eb.HasOne(o => o.DeviceInfo).WithMany(m => m.DeviceServiceProviders).HasForeignKey(f => f.DeviceInfoId);
        }
    }
}
