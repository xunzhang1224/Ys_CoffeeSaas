using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.MarketingActivitys
{
    /// <summary>
    /// 优惠券使用记录实体
    /// 记录优惠券在订单中的使用情况，包含完整的审计信息
    /// </summary>
    public class CouponUsage : EnterpriseBaseEntity
    {
        /// <summary>优惠券ID</summary>
        public long CouponId { get; private set; }

        /// <summary>订单ID</summary>
        public long OrderId { get; private set; }

        /// <summary>活动ID（冗余字段，便于查询）</summary>
        public long CampaignId { get; private set; }

        /// <summary>用户ID</summary>
        public long UserId { get; private set; }

        /// <summary>订单金额（使用优惠券前的金额）</summary>
        public decimal OrderAmount { get; private set; }

        /// <summary>折扣金额</summary>
        public decimal DiscountAmount { get; private set; }

        /// <summary>实际支付金额（订单金额 - 折扣金额）</summary>
        public decimal ActualPaymentAmount { get; private set; }

        /// <summary>使用时间</summary>
        public DateTime UsedTime { get; private set; }

        /// <summary>使用状态</summary>
        public CouponUsageStatus Status { get; private set; }

        /// <summary>取消时间（如果优惠券使用被取消）</summary>
        public DateTime? CanceledTime { get; private set; }

        /// <summary>取消原因</summary>
        public string CancelReason { get; private set; }

        /// <summary>使用的优惠券代码（冗余字段，便于查询和审计）</summary>
        public string CouponCode { get; private set; }

        /// <summary>订单编号（冗余字段）</summary>
        public string OrderNumber { get; private set; }

        /// <summary>使用的商品信息（JSON格式，记录使用时的商品快照）</summary>
        public string UsedProductsInfo { get; private set; }

        /// <summary>使用IP地址（用于风控）</summary>
        public string IpAddress { get; private set; }

        /// <summary>用户设备信息</summary>
        public string UserAgent { get; private set; }

        /// <summary>
        /// 优惠劵
        /// </summary>
        public Coupon Coupon { get; private set; }

        /// <summary>
        /// 优惠劵消费记录
        /// </summary>
        /// <param name="couponId"></param>
        /// <param name="orderId"></param>
        /// <param name="campaignId"></param>
        /// <param name="userId"></param>
        /// <param name="orderAmount"></param>
        /// <param name="discountAmount"></param>
        /// <param name="usedTime"></param>
        /// <param name="couponCode"></param>
        /// <param name="orderNumber"></param>
        /// <param name="usedProductsInfo"></param>
        /// <param name="ipAddress"></param>
        /// <param name="userAgent"></param>
        public CouponUsage(
            long couponId,
            long orderId,
            long campaignId,
            long userId,
            decimal orderAmount,
            decimal discountAmount,
            DateTime usedTime,
            string couponCode,
            string orderNumber,
            string usedProductsInfo = null,
            string ipAddress = null,
            string userAgent = null)
        {
            CouponId = couponId;
            OrderId = orderId;
            CampaignId = campaignId;
            UserId = userId;
            OrderAmount = orderAmount;
            DiscountAmount = discountAmount;
            ActualPaymentAmount = orderAmount - discountAmount;
            UsedTime = usedTime;
            Status = CouponUsageStatus.Used;
            CouponCode = couponCode;
            OrderNumber = orderNumber;
            UsedProductsInfo = usedProductsInfo;
            IpAddress = ipAddress;
            UserAgent = userAgent;
        }

        /// <summary>
        /// a
        /// </summary>
        protected CouponUsage() { }

        /// <summary>
        /// 取消优惠券使用（订单取消时调用）
        /// </summary>
        /// <param name="cancelReason">取消原因</param>
        /// <param name="canceledTime">取消时间</param>
        public void Cancel(string cancelReason, DateTime? canceledTime = null)
        {
            if (Status != CouponUsageStatus.Used)
                throw new Exception("只有已使用的优惠券使用记录才能取消");

            if (string.IsNullOrWhiteSpace(cancelReason))
                throw new Exception("取消原因不能为空");

            Status = CouponUsageStatus.Canceled;
            CancelReason = cancelReason;
            CanceledTime = canceledTime ?? DateTime.Now;

            // 发布领域事件
            // AddDomainEvent(new CouponUsageCanceledEvent(Id, CouponId, OrderId));
        }

        /// <summary>
        /// 标记为退款（订单退款时调用）
        /// </summary>
        public void MarkAsRefunded()
        {
            if (Status != CouponUsageStatus.Used)
                throw new Exception("只有已使用的优惠券使用记录才能标记为退款");

            Status = CouponUsageStatus.Refunded;
            CanceledTime = DateTime.Now;
            CancelReason = "订单退款";
        }

        /// <summary>
        /// 检查使用记录是否有效（未被取消或退款）
        /// </summary>
        public bool IsValid()
        {
            return Status == CouponUsageStatus.Used;
        }

        /// <summary>
        /// 获取使用记录的描述信息
        /// </summary>
        public string GetDescription()
        {
            return $"订单 {OrderNumber} 使用优惠券 {CouponCode} 减免 {DiscountAmount:C}";
        }

        /// <summary>
        /// 创建新的优惠券使用记录（工厂方法）
        /// </summary>
        public static CouponUsage Create(
            long couponId,
            long orderId,
            long campaignId,
            long userId,
            decimal orderAmount,
            decimal discountAmount,
            string couponCode,
            string orderNumber,
            string usedProductsInfo = null,
            string ipAddress = null,
            string userAgent = null)
        {
            return new CouponUsage(
                couponId: couponId,
                orderId: orderId,
                campaignId: campaignId,
                userId: userId,
                orderAmount: orderAmount,
                discountAmount: discountAmount,
                usedTime: DateTime.Now,
                couponCode: couponCode,
                orderNumber: orderNumber,
                usedProductsInfo: usedProductsInfo,
                ipAddress: ipAddress,
                userAgent: userAgent
            );
        }
    }
}