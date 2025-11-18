using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Log;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.AggregatesModel.Strategy;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Strategy
{
    /// <summary>
    /// 饮料制作记录
    /// </summary>
    public class DrinkMakeLogEntityConfiguration : IEntityTypeConfiguration<DrinkMakeLog>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<DrinkMakeLog> eb)
        {
            eb.ToTable("DrinkMakeLog");
            //eb.HasKey(t => t.Id);
            //eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.HasKey(e => new { e.DeviceId, e.Timestamp });
            eb.Property(e => e.Timestamp)
                 .HasColumnType("timestamp with time zone")
                 .HasDefaultValueSql("NOW()");

            eb.HasIndex(e => new { e.DeviceId, e.Timestamp }).IsUnique();
            //            eb.HasIndex(e => new { e.Timestamp, e.DeviceId }).IsUnique()
            //.HasDatabaseName("IX_DrinkMakeLog_Timestamp_DeviceId");
        }
    }
}
