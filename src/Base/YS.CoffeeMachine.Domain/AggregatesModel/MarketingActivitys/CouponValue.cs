using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.DatabaseAccessor;

namespace YS.CoffeeMachine.Domain.AggregatesModel.MarketingActivitys
{
    /// <summary>
    /// 优惠券面值值对象
    /// 封装优惠券金额计算逻辑，确保数据不可变
    /// </summary>
    public class CouponValue : ValueObject
    {
        /// <summary>折扣类型（固定金额、百分比）</summary>
        public DiscountType DiscountType { get; private set; }

        /// <summary>折扣值（固定金额或百分比）</summary>
        public decimal Value { get; private set; }

        /// <summary>最大折扣金额限制（针对百分比折扣）</summary>
        public decimal? MaxDiscountAmount { get; private set; }

        /// <summary>
        /// 添加无参构造函数
        /// </summary>
        protected CouponValue()
        {
        }

        /// <summary>
        /// 优惠劵优惠规格
        /// </summary>
        /// <param name="discountType"></param>
        /// <param name="value"></param>
        /// <param name="minOrderAmount"></param>
        /// <param name="maxDiscountAmount"></param>
        /// <exception cref="ArgumentException"></exception>
        public CouponValue(DiscountType discountType, decimal value,decimal maxDiscountAmount = 0)
        {
            // 参数验证
            if (value <= 0)
                throw new ArgumentException("折扣值必须大于0");

            DiscountType = discountType;
            Value = value;
            //MinOrderAmount = minOrderAmount;
            MaxDiscountAmount = maxDiscountAmount;
        }

        /// <summary>
        /// 计算实际折扣金额
        /// </summary>
        /// <param name="orderAmount">订单金额</param>
        /// <returns>实际折扣金额</returns>
        public decimal CalculateDiscount(decimal orderAmount)
        {
            //// 检查最低订单金额限制
            //if (orderAmount < MinOrderAmount)
            //    return 0;

            decimal discount = DiscountType switch
            {
                DiscountType.FixedAmount => Value,  // 固定金额直接返回
                DiscountType.Percentage => orderAmount * Value / 100,  // 百分比计算
                _ => 0
            };

            // 检查最大折扣金额限制
            if (MaxDiscountAmount.HasValue && discount > MaxDiscountAmount.Value)
                discount = MaxDiscountAmount.Value;

            return discount;
        }

        /// <summary>
        /// 值对象相等性比较
        /// </summary>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return DiscountType;
            yield return Value;
            //yield return MinOrderAmount;
            yield return MaxDiscountAmount;
        }
    }
}
