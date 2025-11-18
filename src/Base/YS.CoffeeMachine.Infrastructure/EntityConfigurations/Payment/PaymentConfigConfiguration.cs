using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.Payment;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Payment
{
    /// <summary>
    /// 支付配置
    /// </summary>
    public class PaymentConfigConfiguration : IEntityTypeConfiguration<PaymentConfig>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<PaymentConfig> eb)
        {
            eb.ToTable("PaymentConfig");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.P_PaymentConfigId).IsRequired();
            eb.Property(t => t.Email).IsRequired();
            eb.Property(t => t.PaymentConfigStatue).HasConversion<string>().IsRequired();
            eb.Property(t => t.MerchantCode).IsRequired();
            eb.Property(t => t.PaymentPlatformAppId).IsRequired();
            eb.Property(t => t.PictureUrl).IsRequired();
            eb.Property(t => t.Remark).IsRequired();
            eb.Property(t => t.Enabled).HasConversion<string>().IsRequired();
        }
    }
}
