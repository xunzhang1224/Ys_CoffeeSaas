using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.Consumer.MarketingActivitys;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.AggregatesModel.MarketingActivitys;

namespace YS.CoffeeMachine.Application.Queries.Consumer
{
    /// <summary>
    /// 营销活动接口
    /// </summary>
    public interface IPromotionQueries
    {
        /// <summary>
        /// 获取营销活动接口
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResultDto<PromotionOutput>> GetPromotionPageList(GetPromotionInput request);

        /// <summary>
        /// 可用优惠劵
        /// </summary>
        /// <returns></returns>
        Task<List<Coupon>> GetCouponList(long? enterpriseinfoId = 0);

        /// <summary>
        /// 当前登录用户可领优惠劵列表
        /// </summary>
        /// <returns></returns>
        Task<List<Promotion>> GetPromotionList();
    }
}
