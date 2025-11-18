using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YSCore.Base.DatabaseAccessor;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Devices
{
    /// <summary>
    /// 设备信息
    /// </summary>
    public class DeviceInfoEntityConfiguration : IEntityTypeConfiguration<DeviceInfo>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<DeviceInfo> eb)
        {
            eb.ToTable("DeviceInfo");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.Mid).IsRequired(false);
            eb.Property(t => t.Name).HasMaxLength(64).IsRequired(false);
            eb.Property(t => t.EquipmentNumber).HasMaxLength(64).IsRequired(false);
            eb.Property(t => t.EnterpriseinfoId).IsRequired();
            eb.Property(t => t.DeviceModelId).IsRequired(false);
            eb.Property(t => t.VersionNumber).HasMaxLength(64).IsRequired(false);
            eb.Property(t => t.ActiveTime).IsRequired(false);
            eb.Property(t => t.DeviceActiveState).IsRequired(false);
            eb.Property(t => t.SkinPluginVersion).HasMaxLength(64).IsRequired(false);
            eb.Property(t => t.LanguagePack).HasMaxLength(64).IsRequired(false);
            eb.Property(t => t.UpdateTime).IsRequired(false);
            eb.Property(t => t.SSID).HasMaxLength(64).IsRequired(false);
            eb.Property(t => t.MAC).HasMaxLength(64).IsRequired(false);
            eb.Property(t => t.ICCID).HasMaxLength(64).IsRequired(false);
            eb.Property(t => t.UsedTrafficThisMonth).HasMaxLength(64).IsRequired(false);
            eb.Property(t => t.RemainingTrafficThisMonth).HasMaxLength(64).IsRequired(false);
            eb.Property(t => t.Latitude).HasMaxLength(64).IsRequired(false);
            eb.Property(t => t.Longitude).HasMaxLength(64).IsRequired(false);
            eb.Property(t => t.LatestOnlineTime).IsRequired(false);
            eb.Property(t => t.LatestOfflineTime).IsRequired(false);
            eb.Property(t => t.CountryId).IsRequired(false);
            eb.Property(t => t.CountryRegionIds).IsRequired(false);
            eb.Property(t => t.CountryRegionText).HasMaxLength(256).IsRequired(false);
            eb.Property(t => t.DetailedAddress).HasMaxLength(256).IsRequired(false);
            eb.Property(t => t.UsageScenario).HasConversion<string>().HasMaxLength(30).IsRequired(false);
            eb.Property(t => t.DeviceStatus).HasConversion<int>().IsRequired();
            eb.Property(t => t.POSMachineNumber).HasMaxLength(64).IsRequired(false);
            eb.Property(t => t.Province).HasMaxLength(64).IsRequired(false);
            eb.Property(t => t.City).HasMaxLength(64).IsRequired(false);
            eb.Property(t => t.District).HasMaxLength(64).IsRequired(false);
            eb.Property(t => t.Street).HasMaxLength(64).IsRequired(false);
            eb.Property(t => t.Lat).HasPrecision(18, 10).IsRequired(false);
            eb.Property(t => t.Lng).HasPrecision(18, 10).IsRequired(false);

            // 授权&表主外键关联
            eb.HasMany<AuthorizedDevices>().WithOne(e => e.DeviceInfo).HasForeignKey(e => e.DeviceId).IsRequired();
            // 设备类型&设备关联
            eb.HasOne(e => e.DeviceModel).WithMany(e => e.DeviceInfos).HasForeignKey(e => e.DeviceModelId).IsRequired();
            // 关联设置
            eb.HasOne(si => si.SettingInfo).WithOne(o => o.DeviceInfo).HasForeignKey<SettingInfo>(k => k.DeviceId);
            // 设备&用户关联
            eb.HasMany(t => t.DeviceUserAssociations).WithOne().HasForeignKey(w => w.DeviceId);

            //eb.HasMany(t => t.DeviceHardwares).WithOne().HasForeignKey(w => w.DeviceBaseId);

            // 设备&饮品关联
            eb.HasMany(t => t.BeverageInfos).WithOne(e => e.DeviceInfo).HasForeignKey(w => w.DeviceId);

            // 配置一对多关系：Device 有多个 Assignment
            eb.HasMany(d => d.Assignments)
                  .WithOne(a => a.Device)
                  .HasForeignKey(a => a.DeviceId)
                  .OnDelete(DeleteBehavior.Cascade); // 删除设备时，联删绑定记录
        }
    }
}
