using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.Payment;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Devices
{
    /// <summary>
    /// 设备支付配置
    /// </summary>
    public class DevicePaymentConfigConfiguration : IEntityTypeConfiguration<DevicePaymentConfig>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<DevicePaymentConfig> eb)
        {
            eb.ToTable("DevicePaymentConfig");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.DeviceInfoId).IsRequired(false);
            eb.Property(t => t.PaymentConfigId).IsRequired(false);
            //eb.Property(t => t.Currency).IsRequired(false);
            //eb.Property(t => t.PayWait).IsRequired(false);
            //eb.Property(t => t.CurrencyLocationEnable).HasConversion<string>().IsRequired(false);
            //eb.Property(t => t.PaymentCurrencyLocaton).HasConversion<string>().IsRequired(false);
        }
    }
}
