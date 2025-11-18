using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YS.CoffeeMachine.Application.Dtos.PagingDto;

namespace YS.CoffeeMachine.Application.Queries.IBeverageQueries
{
    /// <summary>
    /// 饮品查询
    /// </summary>
    public interface IBeverageInfoQueries
    {
        /// <summary>
        /// 获取饮品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BeverageInfoDto> GetBeverageInfoAsync(long id);

        /// <summary>
        /// 获取良品信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        Task<PagedResultDto<BeveragePageListDto>> GetBeverageInfoListAsync(QueryRequest request, long deviceId);

        /// <summary>
        /// 获取饮品价格
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        Task<List<PriceInfoDot>> GetPriceInfoListAsync(long deviceId);
    }
}
