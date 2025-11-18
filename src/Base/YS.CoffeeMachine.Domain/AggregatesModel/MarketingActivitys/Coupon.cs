using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics;
using YS.CoffeeMachine.Domain.AggregatesModel.Order;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.MarketingActivitys
{
    /// <summary>
    /// 优惠劵
    /// </summary>
    public class Coupon : EnterpriseBaseEntity, IAggregateRoot
    {
        /// <summary>所属活动ID</summary>
        [Required]
        public long CampaignId { get; private set; }

        /// <summary>绑定用户ID</summary>
        public long UserId { get; private set; }

        /// <summary>
        /// 使用类型
        /// 0:使用时间 1：领取后有效天
        /// </summary>
        public int UseType { get; private set; } = 0;

        /// <summary>优惠券状态</summary>
        public CouponStatusEnum Status { get; private set; }

        /// <summary>生效时间</summary>
        public DateTime? ValidFrom { get; private set; }

        /// <summary>失效时间</summary>
        public DateTime? ValidTo { get; private set; }

        /// <summary>
        /// 有效天数
        /// </summary>
        public int? UseDay { get; private set; }

        /// <summary>使用时间</summary>
        public DateTime? UsedTime { get; private set; }

        /// <summary>订单ID（使用后记录）</summary>
        public long? OrderId { get; private set; }

        /// <summary>
        /// 营销活动
        /// </summary>
        public Promotion Promotion { get; private set; }

        /// <summary>
        /// a
        /// </summary>
        protected Coupon() { }

        /// <summary>
        /// 添加优惠劵
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="code"></param>
        /// <param name="userId"></param>
        /// <param name="validFrom"></param>
        /// <param name="validTo"></param>
        public Coupon(long campaignId, long userId, int useType, DateTime? validFrom, DateTime? validTo, int? useDay)
        {
            CampaignId = campaignId;
            UserId = userId;
            UseType = useType;
            ValidFrom = validFrom;
            ValidTo = validTo;
            Status = CouponStatusEnum.Active;
            UseDay = useDay;
        }

        /// <summary>
        /// 验证优惠券是否有效
        /// </summary>
        public bool IsValid()
        {
            var now = DateTime.UtcNow;
            if (UseType == 0)
            {
                return Status == CouponStatusEnum.Active &&
                       now >= ValidFrom &&
                       now <= ValidTo;
            }
            else
            {
                return Status == CouponStatusEnum.Active && CreateTime.AddDays(UseDay ?? 0) > now;
            }
        }

        /// <summary>
        /// 标记为已使用
        /// </summary>
        public void MarkAsUsed(long? orderId = null)
        {
            Status = CouponStatusEnum.Used;
            UsedTime = DateTime.Now;
            OrderId = orderId;
        }

        /// <summary>
        /// 作废优惠券
        /// </summary>
        public void Invalidate()
        {
            var now = DateTime.UtcNow;

            if (Status == CouponStatusEnum.Active && ((UseType == 0 && now >= ValidTo) || (UseType == 1 && CreateTime.AddDays(UseDay ?? 0) < now)))
            {
                Status = CouponStatusEnum.Expired;
            }
        }

        /// <summary>
        /// 撤回使用的优惠劵
        /// </summary>
        public void ReturnOrderCoupon()
        {
            var now = DateTime.UtcNow;
            OrderId = null;
            UsedTime = null;
            if ((UseType == 0 && now >= ValidTo) || (UseType == 1 && CreateTime.AddDays(UseDay ?? 0) < now))
            {
                Status = CouponStatusEnum.Expired;
            }
            else
            {
                Status = CouponStatusEnum.Active;
            }
        }
    }
}
