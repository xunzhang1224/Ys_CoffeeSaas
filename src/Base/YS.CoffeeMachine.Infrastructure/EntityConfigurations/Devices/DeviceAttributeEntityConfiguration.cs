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
    /// 设备属性
    /// </summary>
    public class DeviceAttributeEntityConfiguration : IEntityTypeConfiguration<DeviceAttribute>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<DeviceAttribute> eb)
        {
            eb.ToTable("DeviceAttribute");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
        }
    }
}
