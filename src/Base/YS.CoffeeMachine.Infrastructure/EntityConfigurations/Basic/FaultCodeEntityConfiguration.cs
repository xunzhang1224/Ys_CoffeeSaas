using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Basic
{
    /// <summary>
    /// 故障码配置
    /// </summary>
    public class FaultCodeEntityConfiguration : IEntityTypeConfiguration<FaultCodeEntity>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<FaultCodeEntity> builder)
        {
            builder.ToTable("FaultCode");
            builder.HasKey(t => t.Code);
            builder.Property(t => t.LanCode).HasMaxLength(64).IsRequired();
            builder.Property(t => t.Code).HasMaxLength(50).IsRequired();
            builder.Property(t => t.Name).HasMaxLength(128).IsRequired();
            builder.Property(t => t.Description).HasMaxLength(512).IsRequired(false);
            builder.Property(t => t.Remark).HasMaxLength(512).IsRequired(false);
        }
    }
}