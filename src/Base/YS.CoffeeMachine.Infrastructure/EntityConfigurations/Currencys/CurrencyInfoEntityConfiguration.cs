using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.Currencys;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Currencys
{
    /// <summary>
    /// 货币信息
    /// </summary>
    public class CurrencyInfoEntityConfiguration : IEntityTypeConfiguration<CurrencyInfo>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<CurrencyInfo> eb)
        {
            eb.ToTable("CurrencyInfo");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.Culture).HasMaxLength(64).IsRequired();
            eb.Property(t => t.CurrencySymbol).HasMaxLength(256).IsRequired();
            eb.Property(t => t.CurrencyCode).HasMaxLength(64).IsRequired();
            eb.Property(t => t.RegionName).HasMaxLength(256).IsRequired();
            eb.Property(t => t.CountryName).HasMaxLength(256).IsRequired();
        }
    }
}
