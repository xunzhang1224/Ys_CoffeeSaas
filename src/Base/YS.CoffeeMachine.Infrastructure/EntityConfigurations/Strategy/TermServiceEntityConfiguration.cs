using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Strategy;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Strategy
{
    /// <summary>
    /// 服务条款实体配置
    /// </summary>
    public class TermServiceEntityConfiguration : IEntityTypeConfiguration<TermServiceEntity>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<TermServiceEntity> eb)
        {
            eb.ToTable("TermServiceEntity");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.Title).HasMaxLength(128).IsRequired();
            eb.Property(t => t.Content).HasColumnType("nvarchar(max)").IsRequired();
            eb.Property(t => t.Description).HasMaxLength(256).IsRequired(false);
            eb.Property(t => t.Enabled).HasConversion<string>().HasMaxLength(30).IsRequired();
        }
    }
}