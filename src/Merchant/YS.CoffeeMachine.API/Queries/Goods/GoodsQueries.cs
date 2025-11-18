using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.API.Queries.Pagings;
using YS.CoffeeMachine.Application.Dtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Queries.IGoods;
using YS.CoffeeMachine.Domain.AggregatesModel.Goods;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Queries.GoodsQueries
{
    /// <summary>
    /// 商品查询服务
    /// </summary>
    public class GoodsQueries(CoffeeMachineDbContext _db) : IGoodsQueries
    {
        /// <summary>
        /// 验证sku
        /// </summary>
        /// <param name="skus"></param>
        /// <returns></returns>
        public async Task<bool> CheckSku(List<string> skus)
        {
            if (await _db.PrivateGoodsRepository.AnyAsync(x => skus.Contains(x.Sku)))
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0108)]);
            return true;
        }

        /// <summary>
        /// 获取私有库
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<PrivateGoodsRepository>> GetPrivateGoodsPage(GoodsDto dto)
        {
            return await _db.PrivateGoodsRepository
                .WhereIf(!string.IsNullOrWhiteSpace(dto.str), x => (x.Name.Contains(dto.str) || x.Sku.Contains(dto.str)))
                .ToPagedListAsync(dto);
        }
    }
}
