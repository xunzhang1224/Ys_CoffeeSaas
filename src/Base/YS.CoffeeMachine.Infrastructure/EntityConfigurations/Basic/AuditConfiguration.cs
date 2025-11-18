using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Commands.BasicCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Docment;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Basic
{
    /// <summary>
    /// 审计
    /// </summary>
    internal class AuditConfiguration : IEntityTypeConfiguration<Audit>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Audit> builder)
        {
            builder.ToTable("Audit");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            builder.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            builder.Property(t => t.KeyValues).HasMaxLength(int.MaxValue).IsRequired();
            builder.Property(t => t.NewValues).HasMaxLength(int.MaxValue).IsRequired(false);
            builder.Property(t => t.OldValues).HasMaxLength(int.MaxValue).IsRequired(false);
        }
    }
}
