using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Settings
{
    /// <summary>
    /// 设置信息
    /// </summary>
    public class SettingInfoEntityConfiguration : IEntityTypeConfiguration<SettingInfo>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<SettingInfo> eb)
        {
            eb.ToTable("SettingInfo");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            eb.Property(t => t.IsShowEquipmentNumber).IsRequired();
            eb.Property(t => t.IsShowEquipmentNumber).IsRequired();
            eb.Property(t => t.InterfaceStylesId).IsRequired();
            eb.Property(t => t.WashType).IsRequired();
            eb.Property(t => t.RegularWashTime).IsRequired(false);
            eb.Property(t => t.WashWarning).IsRequired(false);
            eb.Property(t => t.AfterSalesPhone).IsRequired(false);
            eb.Property(t => t.ExpectedUpdateTime).IsRequired();
            eb.Property(t => t.ScreenBrightness).IsRequired();
            eb.Property(t => t.DeviceSound).IsRequired();
            eb.Property(t => t.AdministratorPwd).IsRequired();
            eb.Property(t => t.ReplenishmentOfficerPwd).IsRequired();
            eb.Property(t => t.StartTime).IsRequired();
            eb.Property(t => t.StartWeek).IsRequired();
            eb.Property(t => t.EndTime).IsRequired();
            eb.Property(t => t.EndWeek).IsRequired();
            eb.Property(t => t.LanguageName).IsRequired();
            eb.Property(t => t.CurrencyCode).IsRequired();

            //关联设备
            eb.HasOne(si => si.DeviceInfo).WithOne(o => o.SettingInfo).HasForeignKey<SettingInfo>(k => k.DeviceId);
            //关联风格
            eb.HasOne(si => si.InterfaceStyles)    // SettringInfo 关联 InterfaceStyles
            .WithMany()                          // InterfaceStyles 无反向集合
            .HasForeignKey(si => si.InterfaceStylesId) // 外键
            .OnDelete(DeleteBehavior.Restrict);   // 级联删除
            //关联料盒
            //eb.HasMany(m => m.MaterialBoxs).WithOne().HasForeignKey(k => k.SettingInfoId).IsRequired();
        }
    }
}
