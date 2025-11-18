using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Beverages
{
    /// <summary>
    /// 商品分类配置
    /// </summary>
    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<ProductCategory> eb)
        {
            eb.ToTable("ProductCategory");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.ParentId).IsRequired(false);
            eb.Property(t => t.Name).HasMaxLength(64).IsRequired();
            eb.Property(t => t.ImageUrl).HasMaxLength(256).IsRequired();
            eb.Property(t => t.IconUrl).HasMaxLength(256).IsRequired(false);
            eb.Property(t => t.ProductCategoryType).HasConversion<string>().HasMaxLength(30).IsRequired();
            eb.Property(t => t.IsEnabled).IsRequired();
            eb.Property(t => t.Sort).IsRequired();
            eb.Property(t => t.Description).HasMaxLength(256).IsRequired(false);
        }
    }
}