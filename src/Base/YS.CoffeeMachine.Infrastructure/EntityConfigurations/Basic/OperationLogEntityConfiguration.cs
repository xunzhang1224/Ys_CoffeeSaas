using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.OperationLog;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Settings
{
    /// <summary>
    /// 操作日志
    /// </summary>
    public class OperationLogEntityConfiguration : IEntityTypeConfiguration<OperationLog>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<OperationLog> builder)
        {
            builder.ToTable("OperationLog");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            builder.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            builder.Property(t => t.RequestUrl).HasMaxLength(int.MaxValue).IsRequired();
            builder.HasMany(t => t.OperationSubLogs)
                   .WithOne(x=>x.OperationLog)
                   .HasForeignKey(ur => ur.OperationLogId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();
        }
    }
}
