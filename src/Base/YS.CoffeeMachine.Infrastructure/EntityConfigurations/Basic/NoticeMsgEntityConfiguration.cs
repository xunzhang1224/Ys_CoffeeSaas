using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.OperationLog;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Settings
{
    /// <summary>
    /// 通知配置
    /// </summary>
    public class NotityMsgEntityConfiguration : IEntityTypeConfiguration<NotityMsg>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<NotityMsg> builder)
        {
            builder.ToTable("NotityMsg");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            builder.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
        }
    }
}
