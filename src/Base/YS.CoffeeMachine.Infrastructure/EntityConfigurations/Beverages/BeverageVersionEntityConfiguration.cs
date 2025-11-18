using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Beverages
{
    /// <summary>
    /// 饮品版本
    /// </summary>
    public class BeverageVersionEntityConfiguration : IEntityTypeConfiguration<BeverageVersion>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<BeverageVersion> eb)
        {
            eb.ToTable("BeverageVersion");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.BeverageInfoId).IsRequired();
            eb.Property(t => t.VersionType).HasConversion<string>().HasMaxLength(30).IsRequired();
            eb.Property(t => t.BeverageInfoDataString).HasColumnType("nvarchar(max)").IsRequired();

            //关联饮品
            eb.HasOne<BeverageInfo>()
            .WithMany(e => e.BeverageVersions)
            .HasForeignKey(e => e.BeverageInfoId)
            .IsRequired();
        }
    }
}
