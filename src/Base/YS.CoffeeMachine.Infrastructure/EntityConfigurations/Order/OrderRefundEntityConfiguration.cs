using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YS.CoffeeMachine.Domain.AggregatesModel.Order;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Infrastructure.EntityConfigurations.Order
{
    /// <summary>
    /// 退款订单信息
    /// </summary>
    public class OrderRefundEntityConfiguration : IEntityTypeConfiguration<OrderRefund>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<OrderRefund> builder)
        {
            builder.ToTable("OrderRefund");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedNever().UseSnowFlakeValueGenerator();
            builder.Property(t => t.CreateTime).ValueGeneratedOnAdd().UseUtcDateTimeValueGenerator();

            // OrderId 配置
            builder.Property(e => e.OrderId)
                .HasMaxLength(32)
                .IsRequired()
                .HasComment("主订单号（Order表的OrderId字段）");

            // OrderDetailId 配置
            builder.Property(e => e.OrderDetailId)
                .HasMaxLength(32)
                .IsRequired()
                .HasComment("（OrderDetail表的Id字段）");

            // RefundOrderNo 配置
            builder.Property(e => e.RefundOrderNo)
                .HasMaxLength(128)
                .IsRequired()
                .HasComment("退款的交易订单号（支付平台OrderRefund表的Id）");

            // ItemCode 配置
            builder.Property(e => e.ItemCode)
                .IsRequired(false)
                .HasComment("饮品SKU");

            // ProductId 配置
            builder.Property(e => e.ProductId)
                .IsRequired(false)
                .HasComment("商品表的Id（Product表的Id）");

            // BarCode 配置
            builder.Property(e => e.BarCode)
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("商品条码（Product表的BarCode）");

            // Name 配置
            builder.Property(e => e.Name)
                .HasMaxLength(300)
                .IsRequired(false)
                .HasComment("商品名称（Product表的Name）");

            // MainImage 配置
            builder.Property(e => e.MainImage)
                .HasMaxLength(300)
                .IsRequired(false)
                .HasComment("商品主图（Product表的MainImage）");

            // RefundAmount 配置
            builder.Property(e => e.RefundAmount)
                .HasColumnType("decimal(18,4)")
                .IsRequired()
                .HasDefaultValue(0m)
                .HasComment("退款金额");

            // RefundReason 配置
            builder.Property(e => e.RefundReason)
                .HasMaxLength(100)
                .IsRequired(false)
                .HasComment("退款原因");

            // RefundStatus 配置
            builder.Property(e => e.RefundStatus)
                .IsRequired()
                .HasConversion<string>() // 将枚举转换为字符串存储
                .HasDefaultValue(RefundStatusEnum.Success)
                .HasComment("退款状态");

            // HandlingMethod 配置
            builder.Property(e => e.HandlingMethod)
                .IsRequired()
                .HasConversion<string>() // 将枚举转换为字符串存储
                .HasDefaultValue(HandlingMethodEnum.FullRefund)
                .HasComment("处理方式");

            // OrderCreatedOnUtc 配置
            builder.Property(e => e.OrderCreatedOnUtc)
                .IsRequired()
                .HasComment("订单创建时间（Order表的OrderCreatedOnUtc，一定要保持一致）");

            // InitiationTime 配置
            builder.Property(e => e.InitiationTime)
                .IsRequired()
                .HasComment("退款发起时间");

            // SuccessTime 配置
            builder.Property(e => e.SuccessTime)
                .IsRequired(false)
                .HasComment("退款成功时间");
        }
    }
}