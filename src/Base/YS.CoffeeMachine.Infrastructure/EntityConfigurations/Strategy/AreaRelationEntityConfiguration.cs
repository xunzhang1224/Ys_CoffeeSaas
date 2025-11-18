using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.Strategy;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Strategy
{
    /// <summary>
    /// 区域关系
    /// </summary>
    public class AreaRelationEntityConfiguration : IEntityTypeConfiguration<AreaRelation>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<AreaRelation> eb)
        {
            eb.ToTable("AreaRelation");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.Area).HasMaxLength(64).IsRequired();
            eb.Property(t => t.Country).IsRequired();
            eb.Property(t => t.AreaCode).IsRequired();
            eb.Property(t => t.Language).IsRequired();
            eb.Property(t => t.CurrencyId).IsRequired();
            eb.Property(t => t.TimeZone).IsRequired();
            eb.Property(t => t.TermServiceUrl).IsRequired();
            eb.Property(t => t.Enabled).HasConversion<string>().IsRequired();
        }
    }
}
