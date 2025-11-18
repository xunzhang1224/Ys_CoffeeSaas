using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YSCore.Base.DatabaseAccessor;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Beverages
{
    /// <summary>
    /// 饮品版本配置
    /// </summary>
    public class FormulaInfoEntityConfiguration : IEntityTypeConfiguration<FormulaInfo>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<FormulaInfo> eb)
        {
            eb.ToTable("FormulaInfo");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.BeverageInfoId).IsRequired();
            eb.Property(t => t.MaterialBoxId).IsRequired(false);
            eb.Property(t => t.FormulaType).HasConversion<string>().HasMaxLength(30).IsRequired();
            eb.Property(t => t.MaterialBoxName).HasMaxLength(64).IsRequired(false);
            eb.Property(t => t.SpecsString).HasColumnType("nvarchar(max)").IsRequired();

            //关联饮品
            eb.HasOne<BeverageInfo>()
            .WithMany(e => e.FormulaInfos)
            .HasForeignKey(e => e.BeverageInfoId)
            .IsRequired();

            eb.HasIndex(e => e.MaterialBoxId).IsUnique(false);
        }
    }
}
