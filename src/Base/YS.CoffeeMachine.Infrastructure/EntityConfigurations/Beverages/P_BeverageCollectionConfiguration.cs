using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Beverages
{
    /// <summary>
    /// 聚合根配置
    /// </summary>
    public class P_BeverageCollectionConfiguration : IEntityTypeConfiguration<P_BeverageCollection>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<P_BeverageCollection> eb)
        {
            eb.ToTable("P_BeverageCollection");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.LanguageKey).IsRequired();
            eb.Property(t => t.DeviceModelId).IsRequired();
            eb.Property(t => t.Name).HasMaxLength(64).IsRequired();
            eb.Property(t => t.BeverageIds).HasColumnType("nvarchar(max)").IsRequired();
            eb.Property(t => t.BeverageNames).HasColumnType("nvarchar(max)").IsRequired();
        }
    }
}
