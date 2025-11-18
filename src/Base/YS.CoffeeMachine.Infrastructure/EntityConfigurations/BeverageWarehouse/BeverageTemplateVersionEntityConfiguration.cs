using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.BeverageWarehouse;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.BeverageWarehouse
{
    /// <summary>
    /// 饮料信息模板
    /// </summary>
    public class BeverageTemplateVersionEntityConfiguration : IEntityTypeConfiguration<BeverageTemplateVersion>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<BeverageTemplateVersion> eb)
        {
            eb.ToTable("BeverageTemplateVersion");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.BeverageInfoTemplateId).IsRequired();
            eb.Property(t => t.VersionType).HasConversion<string>().HasMaxLength(30).IsRequired();
            eb.Property(t => t.BeverageInfoDataString).HasColumnType("nvarchar(max)").IsRequired();

            //关联饮品
            eb.HasOne<BeverageInfoTemplate>()
            .WithMany(e => e.BeverageTemplateVersions)
            .HasForeignKey(e => e.BeverageInfoTemplateId)
            .IsRequired();
        }
    }
}
