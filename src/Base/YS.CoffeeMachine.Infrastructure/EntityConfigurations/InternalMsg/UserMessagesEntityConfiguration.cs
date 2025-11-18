using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.InternalMsg;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.InternalMsg
{
    /// <summary>
    /// 系统消息
    /// </summary>
    public class UserMessagesEntityConfiguration : IEntityTypeConfiguration<UserMessages>
    {
        /// <summary>
        /// 系统消息
        /// </summary>
        /// <param name="e"></param>
        public void Configure(EntityTypeBuilder<UserMessages> e)
        {
            e.ToTable("UserMessages");

            e.HasKey(x => x.Id);
            e.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            e.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            e.Property(x => x.UserId).HasMaxLength(100).IsRequired();
            e.HasIndex(x => new { x.UserId, x.IsRead, x.IsPopupShown });

            e.HasOne(x => x.Message)
             .WithMany()
             .HasForeignKey(x => x.MessageId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}