using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Beverages
{
    /// <summary>
    /// 饮料集合
    /// </summary>
    public class BeverageCollectionConfiguration : IEntityTypeConfiguration<BeverageCollection>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<BeverageCollection> eb)
        {
            eb.ToTable("BeverageCollection");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.EnterpriseInfoId).IsRequired();
            eb.Property(t => t.DeviceModelId).IsRequired();
            eb.Property(t => t.Name).HasMaxLength(64).IsRequired();
            eb.Property(t => t.BeverageIds).HasColumnType("nvarchar(max)").IsRequired();
            eb.Property(t => t.BeverageNames).HasColumnType("nvarchar(max)").IsRequired();
        }
    }
}
