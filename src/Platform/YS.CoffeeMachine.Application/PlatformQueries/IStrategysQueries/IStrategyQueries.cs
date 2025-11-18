using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Dtos.TermServiceDtos;
using YS.CoffeeMachine.Application.PlatformDto.StrategyDtos;

namespace YS.CoffeeMachine.Application.PlatformQueries.StrategyQueries
{
    /// <summary>
    /// 地区关联查询
    /// </summary>
    public interface IStrategyQueries
    {
        /// <summary>
        /// 获取地区关联分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResultDto<AreaRelationDto>> GetAreaRelationList(QueryRequest request);

        /// <summary>
        /// 获取企业所需地区关联列表
        /// </summary>
        /// <returns></returns>
        Task<List<AreaRelationDto>> GetAreaRelationAllList();

        /// <summary>
        /// 获取服务条款分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResultDto<TermServiceOutput>> GetTermServicePageList(TermServiceInput request);

        /// <summary>
        /// 服务条款选择列表
        /// </summary>
        /// <returns></returns>
        Task<List<TermServiceSelectOutput>> GetTermServiceSelectList();

        /// <summary>
        /// 根据Id获取单个服务条款信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SingleTermServiceOutput> GetSingleTermServiceById(long id);
    }
}