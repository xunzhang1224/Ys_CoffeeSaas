using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Devices
{
    /// <summary>
    /// 设备初始化信息
    /// </summary>
    public class DeviceInitializationEntityConfiguration : IEntityTypeConfiguration<DeviceInitialization>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<DeviceInitialization> eb)
        {
            eb.ToTable("DeviceInitialization");
            eb.HasKey(t => t.Mid);
            eb.Property(t => t.IMEI).IsRequired(false);
            eb.Property(t => t.EquipmentNumber).IsRequired(false);
            eb.Property(t => t.ChanneId).IsRequired(false);
            eb.Property(t => t.PriKey).IsRequired();
            eb.Property(t => t.PubKey).IsRequired();
        }
    }
}
