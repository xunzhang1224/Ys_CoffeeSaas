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
    /// 使用规则值对象
    /// 封装优惠券使用条件的业务规则
    /// </summary>
    public class UsageRules : ValueObject
    {
        /// <summary>
        /// 适用商品的类型
        /// </summary>
        public ApplicableProductsTypeEnum Type { get; private set; }

        /// <summary>饮品列表</summary>
        public List<long> Drinks { get; private set; }

        /// <summary>
        /// 最低订单金额限制
        /// 0:无门槛
        /// </summary>
        public decimal MinOrderAmount { get; private set; } = 0;

        /// <summary>是否与其他优惠同享</summary>
        public bool CanCombineWithOtherOffers { get; private set; }

        /// <summary>
        /// 1
        /// </summary>
        protected UsageRules() { }

        /// <summary>
        /// a
        /// </summary>
        /// <param name="applicableCategories"></param>
        /// <param name="excludedCategories"></param>
        /// <param name="totalLimit"></param>
        /// <param name="canCombineWithOtherOffers"></param>
        public UsageRules(ApplicableProductsTypeEnum? type= null,
                         List<long> drinks = null,
                         bool canCombineWithOtherOffers = false)
        {
            Type = type ?? ApplicableProductsTypeEnum.All;
            Drinks = drinks ?? new List<long>();
            CanCombineWithOtherOffers = canCombineWithOtherOffers;
        }

        /// <summary>
        /// 验证订单是否满足使用条件
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="orderAmount">订单金额</param>
        /// <param name="goodIds">订单商品集合</param>
        /// <returns></returns>
        public bool Validate(PromotionType type, decimal orderAmount, List<long>? goodIds = null)
        {
            var result = false;
            if (type == PromotionType.Coupon)
            {
                // 判断订单商品
                result = Type switch
                {
                    ApplicableProductsTypeEnum.All => true,
                    ApplicableProductsTypeEnum.Available => Drinks.Any(x => goodIds.Contains(x)) ? true : false,
                    ApplicableProductsTypeEnum.NotAvailable => Drinks.Any(x => goodIds.Contains(x)) ? true : false,
                    _ => false
                };
            }

            // 判断消费是否满足最低限额
            result = orderAmount > MinOrderAmount;
            return result;
        }

        /// <summary>
        /// a
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Type;
            yield return string.Join(",", Drinks.OrderBy(x => x));
            yield return CanCombineWithOtherOffers;
        }
    }
}
