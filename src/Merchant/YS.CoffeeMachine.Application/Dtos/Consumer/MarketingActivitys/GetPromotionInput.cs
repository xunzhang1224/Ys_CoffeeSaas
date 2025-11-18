using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.Consumer.MarketingActivitys
{
    /// <summary>
    /// 营销活动列表
    /// </summary>
    public class GetPromotionInput : QueryRequest
    {
        /// <summary>
        /// 活动名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 活动类型
        /// </summary>
        public PromotionType? PromotionType { get; set; }
    }
}
