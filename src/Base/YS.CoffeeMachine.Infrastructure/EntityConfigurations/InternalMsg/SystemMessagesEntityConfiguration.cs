using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.InternalMsg;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.InternalMsg
{
    /// <summary>
    /// 系统消息
    /// </summary>
    public class SystemMessagesEntityConfiguration : IEntityTypeConfiguration<SystemMessages>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="e"></param>
        public void Configure(EntityTypeBuilder<SystemMessages> e)
        {
            e.ToTable("SystemMessages");

            e.HasKey(x => x.Id);
            e.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            e.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            e.Property(x => x.Title).HasMaxLength(100).IsRequired();
            e.Property(x => x.Content).HasColumnType("nvarchar(max)").IsRequired();
            e.Property(x => x.MessageType).IsRequired();
            e.Property(x => x.TargetUserId).IsRequired(false);
            e.Property(x => x.TargetGroup).HasMaxLength(500).IsRequired(false);
        }
    }
}