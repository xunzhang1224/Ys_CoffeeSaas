using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Beverages
{
    /// <summary>
    /// 饮料集合
    /// </summary>
    public class BeverageInfoEntityConfiguration : IEntityTypeConfiguration<BeverageInfo>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<BeverageInfo> eb)
        {
            eb.ToTable("BeverageInfo");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.DeviceId).IsRequired();
            eb.Property(t => t.CategoryIds).IsRequired(false);
            eb.Property(t => t.Name).HasMaxLength(64).IsRequired();
            eb.Property(t => t.Price).HasPrecision(18, 4).IsRequired(false);
            eb.Property(t => t.DiscountedPrice).HasPrecision(18, 4).IsRequired(false);
            eb.Property(t => t.BeverageIcon).HasMaxLength(256).IsRequired();
            eb.Property(t => t.Code).HasMaxLength(64).IsRequired();
            eb.Property(t => t.Temperature).HasConversion<string>().HasMaxLength(30).IsRequired();
            eb.Property(t => t.Remarks).HasMaxLength(256).IsRequired(false);
            eb.Property(t => t.ProductionForecast).IsRequired(false);
            eb.Property(t => t.ForecastQuantity).IsRequired();
            eb.Property(t => t.IsDefault).IsRequired();
            eb.Property(t => t.DisplayStatus).IsRequired();
            eb.Property(t => t.Sort).IsRequired(false);

            // 设置多字段唯一键
            //eb.HasIndex(e => new { e.DeviceId, e.Code }).IsUnique();

            //关联配方
            eb.HasMany(e => e.FormulaInfos)
            .WithOne()
            .HasForeignKey(e => e.BeverageInfoId)
            .IsRequired();
            //关联历史
            eb.HasMany(e => e.BeverageVersions)
            .WithOne()
            .HasForeignKey(e => e.BeverageInfoId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        }
    }
}