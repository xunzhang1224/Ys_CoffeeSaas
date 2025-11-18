using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.InternalMsg;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.InternalMsg
{
    /// <summary>
    /// 系统消息
    /// </summary>
    public class UserReadGlobalMessagesEntityConfiguration : IEntityTypeConfiguration<UserReadGlobalMessages>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="e"></param>
        public void Configure(EntityTypeBuilder<UserReadGlobalMessages> e)
        {
            e.ToTable("UserReadGlobalMessages");

            e.HasKey(x => x.Id);
            e.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            e.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            e.Property(x => x.UserId).IsRequired();
            e.HasIndex(x => new { x.UserId, x.MessageId }).IsUnique();

            e.HasOne(x => x.Message)
             .WithMany()
             .HasForeignKey(x => x.MessageId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}