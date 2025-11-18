using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// 任务调度信息
    /// </summary>
    public class TaskSchedulingInfoEntityConfiguration : IEntityTypeConfiguration<TaskSchedulingInfo>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<TaskSchedulingInfo> eb)
        {
            eb.ToTable("TaskSchedulingInfo");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.Name).HasMaxLength(64).IsRequired();
            eb.Property(t => t.CronExpression).HasMaxLength(64).IsRequired();
            eb.Property(t => t.Description).HasMaxLength(512).IsRequired(false);
        }
    }
}
