using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Domain.AggregatesModel.ConsumerEntitys;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.ConsumerEntitys
{
    /// <summary>
    /// 消费者端用户表配置
    /// </summary>
    public class ClientUserEntityConfiguration : IEntityTypeConfiguration<ClientUser>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ClientUser> builder)
        {
            builder.ToTable("ClientUser");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            builder.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            builder.Property(t => t.UserName).HasMaxLength(64);
            builder.Property(t => t.Password).HasMaxLength(int.MaxValue).IsRequired();
            builder.Property(t => t.Phone).HasMaxLength(64).IsRequired(false);
            builder.Property(t => t.Email).HasMaxLength(64).IsRequired();
            builder.HasIndex(t => t.Account).IsUnique();
            builder.HasIndex(t => t.WechatId).IsUnique();
        }
    }
}