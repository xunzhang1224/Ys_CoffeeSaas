using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Devices
{
    /// <summary>
    /// 预警配置
    /// </summary>
    public class EarlyWarningConfigEntityConfiguration : IEntityTypeConfiguration<EarlyWarningConfig>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<EarlyWarningConfig> eb)
        {
            eb.ToTable("EarlyWarningConfig");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.DeviceInfoId).IsRequired();
            eb.Property(t => t.WholeMachineCleaningSwitch).IsRequired();
            eb.Property(t => t.NextWholeMachineCleaningTime).IsRequired();
            eb.Property(t => t.BrewingMachineCleaningSwitch).IsRequired();
            eb.Property(t => t.NextBrewingMachineCleaningTime).IsRequired();
            eb.Property(t => t.MilkFrotherCleaningSwitch).IsRequired();
            eb.Property(t => t.NextMilkFrotherCleaningTime).IsRequired();
            eb.Property(t => t.CoffeeWaterwayCleaningSwitch).IsRequired();
            eb.Property(t => t.NextCoffeeWaterwayCleaningTime).IsRequired();
            eb.Property(t => t.SteamWaterwayCleaningSwitch).IsRequired();
            eb.Property(t => t.NextSteamWaterwayCleaningTime).IsRequired();
            eb.Property(t => t.OfflineWarningSwitch).IsRequired();
            eb.Property(t => t.OfflineDays).IsRequired();
            eb.Property(t => t.ShortageWarningSwitch).IsRequired();
            eb.Property(t => t.CoffeeBeanRemaining).IsRequired();
            eb.Property(t => t.WaterRemaining).IsRequired();

            //关联设备
            eb.HasOne(si => si.DeviceInfo).WithOne(o => o.EarlyWarningConfig).HasForeignKey<EarlyWarningConfig>(k => k.DeviceInfoId);
        }
    }
}
