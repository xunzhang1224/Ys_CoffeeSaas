using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Language;
using YS.CoffeeMachine.Domain.AggregatesModel.MarketingActivitys;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.MarketingActivitys
{
    /// <summary>
    /// a
    /// </summary>
    public class CouponEntityConfiguration : IEntityTypeConfiguration<Coupon>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="eb"></param>
        public void Configure(EntityTypeBuilder<Coupon> eb)
        {
            eb.ToTable("Coupon");
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            eb.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();

            // 配置索引以提高查询性能
            eb.HasIndex(c => c.CampaignId); // 按活动ID查询
            eb.HasIndex(c => c.UserId); // 按用户ID查询
            eb.HasIndex(c => c.Status); // 按状态查询
            eb.HasIndex(c => new { c.UserId, c.Status }); // 组合索引：用户+状态
            eb.HasIndex(c => new { c.Status, c.ValidFrom, c.ValidTo }); // 组合索引：状态+有效期
            eb.HasIndex(c => c.OrderId);

        }
    }
}
