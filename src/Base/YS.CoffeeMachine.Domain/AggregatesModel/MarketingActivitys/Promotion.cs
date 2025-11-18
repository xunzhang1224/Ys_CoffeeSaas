using Aop.Api.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yitter.IdGenerator;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics;
using YS.CoffeeMachine.Domain.AggregatesModel.CountryModels;
using YS.CoffeeMachine.Domain.AggregatesModel.Order;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.DatabaseAccessor;
using static YS.Cabinet.Payment.Alipay.BatchTradeEnum;

namespace YS.CoffeeMachine.Domain.AggregatesModel.MarketingActivitys
{
    /// <summary>
    /// 营销活动
    /// </summary>
    public class Promotion : EnterpriseBaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort { get; private set; }

        /// <summary>活动名称</summary>
        [Required]
        public string Name { get; private set; }

        /// <summary>
        /// 发放的数量
        /// </summary>
        [Required]
        public int Number { get; private set; }

        /// <summary>活动开始时间</summary>
        public DateTime StartTime { get; private set; }

        /// <summary>活动结束时间</summary>
        public DateTime EndTime { get; private set; }

        /// <summary>优惠券类型（满减券、折扣券等）</summary>
        public PromotionType CouponType { get; private set; }

        /// <summary>
        /// 总发放数量
        /// </summary>
        [Required]
        public int TotalLimit { get; private set; }

        /// <summary>
        /// 限领次数
        /// 0：不限次数
        /// </summary>
        [Required]
        public int LimitedCount { get; private set; } = 0;

        /// <summary>
        /// 参与用户
        /// 为空标识全部用户
        /// </summary>
        public List<long>? ParticipatingUsers { get; private set; }

        /// <summary>
        /// 优惠券面值配置
        /// </summary>
        public CouponValue Value { get; private set; }

        /// <summary>
        /// 使用规则
        /// </summary>
        public UsageRules UsageRules { get; private set; }

        /// <summary>
        /// 优惠券列表
        /// </summary>
        public List<Coupon> Coupons { get; private set; }

        /// <summary>活动状态</summary>
        [NotMapped]
        public PromotionStatusEnum PromotionStatus
        {
            get
            {
                var now = DateTime.UtcNow;
                if (now < StartTime)
                    return PromotionStatusEnum.NotStarted;
                else if (now > EndTime || Coupons.Count >= TotalLimit)
                    return PromotionStatusEnum.End;
                else
                    return PromotionStatusEnum.Ing;
            }
        }

        /// <summary>
        /// a
        /// </summary>
        protected Promotion() { }

        /// <summary>
        /// 添加活动
        /// </summary>
        /// <param name="name"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="couponType"></param>
        /// <param name="totalLimit"></param>
        /// <param name="value"></param>
        /// <param name="usageRules"></param>
        /// <param name="limitedCount"></param>
        /// <param name="ParticipatingUsers"></param>
        public Promotion(string name, DateTime startTime, DateTime endTime, PromotionType couponType, int totalLimit,
            CouponValue value, UsageRules usageRules, int limitedCount = 0, int sort = 0, List<long>? ParticipatingUsers = null)
        {
            Name = name;
            StartTime = startTime;
            EndTime = endTime;
            CouponType = couponType;
            TotalLimit = totalLimit;
            Sort = sort;
            Value = value;
            UsageRules = usageRules;
            LimitedCount = limitedCount;
        }

        /// <summary>
        /// 修改活动
        /// </summary>
        /// <param name="name"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="couponType"></param>
        /// <param name="totalLimit"></param>
        /// <param name="value"></param>
        /// <param name="usageRules"></param>
        /// <param name="limitedCount"></param>
        /// <param name="ParticipatingUsers"></param>
        public void Update(string name, DateTime startTime, DateTime endTime, PromotionType couponType, int totalLimit,
            CouponValue value, UsageRules usageRules, int limitedCount = 0, int sort = 0, List<long>? ParticipatingUsers = null)
        {
            Name = name;
            StartTime = startTime;
            EndTime = endTime;
            CouponType = couponType;
            TotalLimit = totalLimit;
            Sort = sort;
            Value = value;
            UsageRules = usageRules;
            LimitedCount = limitedCount;
        }

        /// <summary>
        /// 判断用户是否可领
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsReceive(long userId)
        {
            if (PromotionStatus != PromotionStatusEnum.Ing)
                return false;

            if (ParticipatingUsers != null && !ParticipatingUsers.Any(x => x == userId))
                return false;

            if (DateTime.UtcNow <= StartTime || DateTime.UtcNow >= EndTime)
                return false;

            // 用户领卷次数
            var i = Coupons.Count(x => x.UserId == userId);
            if (LimitedCount > 0 && i >= LimitedCount)
                return false;

            if (TotalLimit <= Coupons.Count)
                return false;
            return true;
        }

        /// <summary>
        /// 用户领取优惠券
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="validFrom"></param>
        /// <param name="validTo"></param>
        /// <param name="useDay"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Coupon GenerateCoupon(long userId, int useType, DateTime? validFrom = null, DateTime? validTo = null, int? useDay = null)
        {
            if (PromotionStatus != PromotionStatusEnum.Ing)
                throw new Exception("只有进行中的活动才能生成优惠券！");

            if (ParticipatingUsers != null && !ParticipatingUsers.Any(x => x == userId))
                throw new Exception("用户不能参与此活动！");

            if (DateTime.UtcNow <= StartTime || DateTime.UtcNow >= EndTime)
                throw new Exception("不在活动有效期内！");

            // 用户领卷次数
            var i = Coupons.Count(x => x.UserId == userId);
            if (LimitedCount > 0 && i >= LimitedCount)
                throw new Exception("已达到限领次数！");

            if (TotalLimit <= Coupons.Count)
                throw new Exception("活动次数已达上限！");

            var coupon = new Coupon(
                campaignId: Id,
                userId: userId,
                useType: useType,
                validFrom: validFrom,
                validTo: validTo,
                useDay: useDay
            );

            Coupons.Add(coupon);

            //AddDomainEvent(new CouponGeneratedEvent(coupon.Id, userId));

            return coupon;
        }

        /// <summary>
        /// 自动给目标用户发放优惠劵
        /// </summary>
        //public void BatchGenerateCoupons(List<long> userIds)
        //{
        //    foreach (long userId in userIds)
        //    {
        //        GenerateCoupon(userId);
        //    }
        //}

        /// <summary>
        /// 验证优惠券是否可用于指定订单
        /// </summary>
        /// <param name="couponId">优惠券ID</param>
        /// <param name="orderAmount">订单金额</param>
        /// <param name="goodIds">订单商品</param>
        /// <returns>验证结果</returns>
        public bool ValidateCouponUsage(long couponId, decimal orderAmount, List<long> goodIds)
        {
            var coupon = Coupons.FirstOrDefault(c => c.Id == couponId);

            // 验证优惠券是否存在且有效
            if (coupon == null || !coupon.IsValid())
                return false;

            // 委托给值对象验证业务规则
            return UsageRules.Validate(CouponType, orderAmount, goodIds);
        }

        /// <summary>
        /// 应用优惠券到订单
        /// </summary>
        /// <param name="couponId">优惠券ID</param>
        /// <param name="orderAmount">订单金额</param>
        /// <param name="orderId">订单ID</param>
        /// <returns>折扣金额</returns>
        /// <exception cref="Exception">优惠券不可用时抛出</exception>
        public decimal ApplyCoupon(long couponId, decimal orderAmount, long orderId)
        {
            var coupon = Coupons.FirstOrDefault(c => c.Id == couponId);
            if (coupon == null)
                throw new Exception("优惠券不存在");

            //if (!coupon.CanUse())
            //    throw new Exception("优惠券不可用");

            // 计算折扣金额
            var discountAmount = Value.CalculateDiscount(orderAmount);
            // 记录使用历史
            //var usage = new CouponUsage(couponId, orderId, orderAmount, discountAmount, DateTime.Now);

            //Usages.Add(usage);
            coupon.MarkAsUsed();

            //AddDomainEvent(new CouponUsedEvent(couponId, orderId, discountAmount));

            return discountAmount;
        }
    }
}
