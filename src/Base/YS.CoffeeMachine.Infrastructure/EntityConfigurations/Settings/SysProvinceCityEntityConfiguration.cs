using Microsoft.EntityFrameworkCore;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Settings
{
    /// <summary>
    /// 标准地区配置
    /// </summary>
    public class SysProvinceCityEntityConfiguration : IEntityTypeConfiguration<Domain.AggregatesModel.Settings.SysProvinceCity>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Domain.AggregatesModel.Settings.SysProvinceCity> builder)
        {
            builder.ToTable("SysProvinceCity");
            builder.HasNoKey();
            builder.Property(t => t.Name).HasMaxLength(50);
            builder.Property(t => t.Code).HasMaxLength(50);
            builder.Property(t => t.ParentCode).HasMaxLength(50);
            builder.Property(t => t.Type).HasMaxLength(50);
        }
    }
}