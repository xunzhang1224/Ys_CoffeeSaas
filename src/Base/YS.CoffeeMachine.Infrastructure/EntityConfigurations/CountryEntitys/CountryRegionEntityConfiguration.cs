using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.CountryModels;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.CountryEntitys
{
    /// <summary>
    /// 国家信息
    /// </summary>
    public class CountryRegionEntityConfiguration : IEntityTypeConfiguration<CountryRegion>
    {
        /// <summary>
        /// 配置表结构
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<CountryRegion> eb)
        {
            eb.ToTable("CountryRegion");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.RegionName).HasMaxLength(128).IsRequired();
            eb.Property(t => t.CountryID).IsRequired();
            eb.Property(t => t.IsEnabled).HasConversion<string>().HasMaxLength(30).IsRequired();
            eb.Property(t => t.Sort).IsRequired();

            // 配置自关联关系
            eb.HasOne(x => x.ParentRegion) // 父级导航属性
                .WithMany(x => x.Regions) // 子集合导航属性
                .HasForeignKey(x => x.ParentID) // 外键
                .OnDelete(DeleteBehavior.Restrict); // 级联删除

            eb.HasOne(cr => cr.CountryInfo)
            .WithMany(ci => ci.Regions)
            .HasForeignKey(cr => cr.CountryID);
        }
    }
}
