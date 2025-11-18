using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.CountryModels;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.CountryEntitys
{
    /// <summary>
    /// 国家信息
    /// </summary>
    public class CountryInfoEntityConfiguration : IEntityTypeConfiguration<CountryInfo>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<CountryInfo> eb)
        {
            eb.ToTable("CountryInfo");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.CountryName).HasMaxLength(64).IsRequired();
            eb.Property(t => t.CountryCode).HasMaxLength(64).IsRequired();
            eb.Property(t => t.IsEnabled).HasConversion<string>().HasMaxLength(30).IsRequired();

            //国家关联地区
            eb.HasMany(t => t.Regions).WithOne(o => o.CountryInfo).HasForeignKey(k => k.CountryID).IsRequired();
        }
    }
}
