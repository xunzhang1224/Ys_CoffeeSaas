using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.OperationLog;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Settings
{
    /// <summary>
    /// 操作子日志
    /// </summary>
    public class OperationSubLogEntityConfiguration : IEntityTypeConfiguration<OperationSubLog>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<OperationSubLog> builder)
        {
            builder.ToTable("OperationSubLog");
            builder.HasKey(t => new { t.Mid, t.OperationLogId });
            builder.Property(t => t.ErrorMsg).IsRequired(false);
            builder.Property(t => t.AppliedType).IsRequired(false);
            builder.Property(t => t.ContentType).IsRequired(false);
            builder.Property(t => t.ReplaceTarget).IsRequired(false);
            builder.Property(e => e.RequestMsg)
              .HasColumnType("nvarchar(MAX)")
            .IsUnicode(true)
            .IsRequired(false);

        }
    }
}
