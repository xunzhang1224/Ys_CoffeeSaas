using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.BeverageWarehouse;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.BeverageWarehouse
{
    /// <summary>
    /// 饮料信息模板
    /// </summary>
    public class FormulaInfoTemplateEntityConfiguration : IEntityTypeConfiguration<FormulaInfoTemplate>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<FormulaInfoTemplate> eb)
        {
            eb.ToTable("FormulaInfoTemplate");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.BeverageInfoTemplateId).IsRequired();
            eb.Ignore(f => f.MaterialBox).Property(t => t.MaterialBoxId).HasColumnName("MaterialBoxId").IsRequired(false);
            eb.Property(t => t.MaterialBoxName).HasMaxLength(64).IsRequired(false);
            eb.Property(t => t.SpecsString).HasColumnType("nvarchar(max)").IsRequired();

            //关联饮品
            eb.HasOne<BeverageInfoTemplate>()
            .WithMany(e => e.FormulaInfoTemplates)
            .HasForeignKey(e => e.BeverageInfoTemplateId)
            .IsRequired();
        }
    }
}
