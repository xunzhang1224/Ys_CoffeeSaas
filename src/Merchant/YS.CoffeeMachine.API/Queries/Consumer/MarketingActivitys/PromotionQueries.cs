using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.API.Queries.Pagings;
using YS.CoffeeMachine.Application.Dtos.Consumer.MarketingActivitys;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Queries.Consumer;
using YS.CoffeeMachine.Domain.AggregatesModel.Advertisements;
using YS.CoffeeMachine.Domain.AggregatesModel.MarketingActivitys;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;

namespace YS.CoffeeMachine.API.Queries.Consumer.MarketingActivitys
{
    /// <summary>
    /// 营销活动
    /// </summary>
    public class PromotionQueries(CoffeeMachineDbContext _db, IMapper mapper,UserHttpContext _user) : IPromotionQueries
    {
        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResultDto<PromotionOutput>> GetPromotionPageList(GetPromotionInput request)
        {
            var datas = await _db.Promotion.Include(x => x.Coupons)
                .WhereIf(!string.IsNullOrWhiteSpace(request.Name), x => x.Name.Contains(request.Name))
                .WhereIf(request.PromotionType != null, x => x.CouponType == request.PromotionType)
                .OrderByDescending(x => x.CreateTime)
                .ToPagedListAsync(request);
            return mapper.Map<PagedResultDto<PromotionOutput>>(datas);
        }

        /// <summary>
        /// 可领取的优惠劵列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<Promotion>> GetPromotionList()
        {
            var result = new List<Promotion>();
            var datas = await _db.Promotion.IgnoreQueryFilters().Include(x => x.Coupons).ToListAsync();
            if (datas != null && datas.Any())
            {
                foreach (var item in datas)
                {
                    if (item.IsReceive(_user.UserId))
                    {
                        result.Add(item);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 可用优惠劵
        /// </summary>
        /// <param name="enterpriseinfoId">不为空时获取指定租户下的优惠劵</param>
        /// <returns></returns>
        public async Task<List<Coupon>> GetCouponList(long? enterpriseinfoId = 0)
        {
            return await _db.Coupon.IgnoreQueryFilters().Include(x => x.Promotion)
                .Where(x => x.UserId == _user.UserId && x.Status == CouponStatusEnum.Active)
                .WhereIf(enterpriseinfoId != null && enterpriseinfoId > 0, x => x.EnterpriseinfoId == enterpriseinfoId).ToListAsync();
        }
    }
}
