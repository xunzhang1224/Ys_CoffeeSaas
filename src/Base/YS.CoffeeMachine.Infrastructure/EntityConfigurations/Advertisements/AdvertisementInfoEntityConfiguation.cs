using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.Advertisements;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Advertisements
{
    /// <summary>
    /// 广告信息
    /// </summary>
    public class AdvertisementInfoEntityConfiguation : IEntityTypeConfiguration<AdvertisementInfo>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<AdvertisementInfo> eb)
        {
            eb.ToTable("AdvertisementInfo");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();
            //eb.Property(t => t.DeviceInfoId).IsRequired();
            eb.Property(t => t.PowerOnAdsVolume).IsRequired();
            eb.Property(t => t.PowerOnAdsPlayTime).IsRequired();
            eb.Property(t => t.PowerOnAdsImagesJson).IsRequired();
            eb.Property(t => t.StandbyAdsVolume).IsRequired();
            eb.Property(t => t.StandbyAdsPlayTime).IsRequired();
            eb.Property(t => t.StandbyAdsAwaitTime).IsRequired();
            eb.Property(t => t.StandbyAdsLoopTime).IsRequired();
            eb.Property(t => t.StandbyAdsLoopType).IsRequired();
            eb.Property(t => t.StandbyAdsImagesJson).IsRequired();
            eb.Property(t => t.ProductionAdsVolume).IsRequired();
            eb.Property(t => t.ProductionAdsPlayTime).IsRequired();
            eb.Property(t => t.ProductionAdsImagesJson).IsRequired();

            //关联设备
            eb.HasOne(si => si.DeviceInfo).WithOne(o => o.AdvertisementInfo).HasForeignKey<AdvertisementInfo>(k => k.DeviceInfoId);
        }
    }
}
