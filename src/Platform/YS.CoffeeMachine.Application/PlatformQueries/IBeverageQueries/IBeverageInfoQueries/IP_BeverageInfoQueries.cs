using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.PlatformDto.BeverageInfoDtos;

namespace YS.CoffeeMachine.Application.PlatformQueries.IBeverageQueries.IBeverageInfoQueries
{
    /// <summary>
    /// 饮品查询
    /// </summary>
    public interface IP_BeverageInfoQueries
    {
        //Task<BeverageInfoDto> GetBeverageInfoAsync(long id);
        //Task<PagedResultDto<BeveragePageListDto>> GetBeverageInfoListAsync(QueryRequest request, long deviceId);

        /// <summary>
        /// 查询饮品分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResultDto<P_BeverageInfoDto>> GetBeverageInfoListAsync(BeverageInfoInput request);

        /// <summary>
        /// 查询历史版本列表
        /// </summary>
        /// <param name="beverageInfoId"></param>
        /// <returns></returns>
        Task<List<P_BeverageVersionDto>> GetBeverageVersionListAsync(long beverageInfoId);
    }
}
