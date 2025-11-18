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
    public class P_PaymentConfigConfiguration : IEntityTypeConfiguration<P_PaymentConfig>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<P_PaymentConfig> eb)
        {
            eb.ToTable("P_PaymentConfig");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.Name).IsRequired();
            eb.Property(t => t.Countrys).IsRequired();
            eb.Property(t => t.PaymentModel).HasConversion<string>().IsRequired();
            eb.Property(t => t.PictureUrl).IsRequired();
            eb.Property(t => t.Enabled).HasConversion<string>().IsRequired();
        }
    }
}
