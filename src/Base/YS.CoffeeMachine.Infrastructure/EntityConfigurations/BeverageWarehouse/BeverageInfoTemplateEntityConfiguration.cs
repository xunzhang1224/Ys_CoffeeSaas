using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.BeverageWarehouse;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.BeverageWarehouse
{
    /// <summary>
    /// 饮料信息模板
    /// </summary>
    public class BeverageInfoTemplateEntityConfiguration : IEntityTypeConfiguration<BeverageInfoTemplate>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<BeverageInfoTemplate> eb)
        {
            eb.ToTable("BeverageInfoTemplate");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.EnterpriseInfoId).IsRequired(false);
            eb.Property(t => t.DeviceModelId).IsRequired();
            eb.Property(t => t.CategoryIds).IsRequired(false);
            eb.Property(t => t.Name).HasMaxLength(64).IsRequired();
            eb.Property(t => t.BeverageIcon).HasMaxLength(256).IsRequired();
            eb.Property(t => t.Code).HasMaxLength(64).IsRequired();
            eb.Property(t => t.Remarks).HasMaxLength(256).IsRequired(false);
            eb.Property(t => t.ProductionForecast).IsRequired();
            eb.Property(t => t.ForecastQuantity).IsRequired();
            eb.Property(t => t.DisplayStatus).IsRequired();
            eb.Property(t => t.SellStradgy).IsRequired(false);

            //关联配方
            eb.HasMany(e => e.FormulaInfoTemplates)
            .WithOne()
            .HasForeignKey(e => e.BeverageInfoTemplateId)
            .IsRequired();
            //关联历史版本
            eb.HasMany(e => e.BeverageTemplateVersions)
            .WithOne()
            .HasForeignKey(e => e.BeverageInfoTemplateId)
            .IsRequired();
        }
    }
}
