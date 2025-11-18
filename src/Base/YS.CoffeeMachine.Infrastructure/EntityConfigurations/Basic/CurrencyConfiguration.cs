using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Basic
{
    /// <summary>
    /// 货币信息
    /// </summary>
    internal class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.ToTable("Currency");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            builder.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            builder.Property(t => t.Code).IsRequired();
            builder.Property(t => t.Name).IsRequired();
            builder.Property(t => t.CurrencySymbol).IsRequired();
            builder.Property(t => t.CurrencyShowFormat).HasConversion<string>().IsRequired();
            builder.Property(t => t.Accuracy).IsRequired();
            builder.Property(t => t.RoundingType).HasConversion<string>().IsRequired();
            builder.Property(t => t.Enabled).HasConversion<string>().IsRequired();
        }
    }
}
