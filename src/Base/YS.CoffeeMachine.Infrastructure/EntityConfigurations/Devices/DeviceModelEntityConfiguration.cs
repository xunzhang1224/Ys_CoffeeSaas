using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Devices
{
    /// <summary>
    /// 设备型号
    /// </summary>
    public class DeviceModelEntityConfiguration : IEntityTypeConfiguration<DeviceModel>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<DeviceModel> eb)
        {
            eb.ToTable("DeviceModel");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.Key).HasMaxLength(64).IsRequired();
            eb.Property(t => t.Name).HasMaxLength(64).IsRequired();
            eb.Property(t => t.MaxCassetteCount).IsRequired();
            eb.Property(t => t.Remark).HasMaxLength(512).IsRequired();

            //设备类型&设备关联
            eb.HasMany(e => e.DeviceInfos).WithOne(e => e.DeviceModel).HasForeignKey(e => e.DeviceModelId).IsRequired();

            //设置Key唯一索引
            eb.HasIndex(e => e.Key).IsUnique().HasDatabaseName("IX_DeviceModel_Key");
        }
    }
}