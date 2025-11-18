using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel;
using YS.CoffeeMachine.Domain.AggregatesModel.MarketingActivitys;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.MarketingActivitys
{
    /// <summary>
    /// 营销活动
    /// </summary>
    public class PromotionEntityConfiguration : IEntityTypeConfiguration<Promotion>
    {
        /// <summary>
        /// a
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Promotion> builder)
        {
            // 设置表名
            builder.ToTable("Promotion");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            builder.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();

            // 配置各属性
            builder.Property(p => p.Sort)
                .IsRequired(false);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.ParticipatingUsers)
                .HasConversion(
                    v => v == null ? null : JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => v == null ? null : JsonSerializer.Deserialize<List<long>>(v, (JsonSerializerOptions)null),
                    new ValueComparer<List<long>>(
                        (c1, c2) => c1 != null && c2 != null ? c1.SequenceEqual(c2) : c1 == c2,
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c.ToList()
                    )
                )
                .HasColumnType("nvarchar(max)")
                .IsRequired(false);

            builder.OwnsOne(p => p.Value, valueBuilder =>
            {
                valueBuilder.Property(v => v.DiscountType)
                    .IsRequired()
                    .HasConversion<int>()
                    .HasColumnName("DiscountType");

                valueBuilder.Property(v => v.Value)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)")
                    .HasColumnName("DiscountValue");

                valueBuilder.Property(v => v.MaxDiscountAmount)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired(false)
                    .HasColumnName("MaxDiscountAmount");
            });

            builder.OwnsOne(p => p.UsageRules, rulesBuilder =>
            {
                rulesBuilder.Property(r => r.Type)
                    .IsRequired()
                    .HasConversion<int>()
                    .HasColumnName("ApplicableProductsType")
                    .HasDefaultValue(ApplicableProductsTypeEnum.All);

                rulesBuilder.Property(r => r.Drinks)
                    .HasConversion(
                        v => v == null || v.Count == 0 ? null : JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => string.IsNullOrEmpty(v) ? new List<long>() : JsonSerializer.Deserialize<List<long>>(v, (JsonSerializerOptions)null),
                        new ValueComparer<List<long>>(
                            (c1, c2) => c1 != null && c2 != null ? c1.SequenceEqual(c2) : c1 == c2,
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToList()
                        )
                    )
                    .HasColumnType("nvarchar(max)")
                    .IsRequired(false)
                    .HasColumnName("ApplicableDrinks");

                rulesBuilder.Property(r => r.MinOrderAmount)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0m)
                    .HasColumnName("MinOrderAmount");

                rulesBuilder.Property(r => r.CanCombineWithOtherOffers)
                    .IsRequired()
                    .HasDefaultValue(false)
                    .HasColumnName("CanCombineWithOtherOffers");
            });

            builder.HasMany(p => p.Coupons)
                .WithOne(x => x.Promotion)
                 .HasForeignKey(ur => ur.CampaignId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => p.StartTime);
            builder.HasIndex(p => p.EndTime);
            builder.HasIndex(p => new { p.StartTime, p.EndTime });
        }
    }
}
