using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YS.CoffeeMachine.Application.Dtos.PagingDto;

namespace YS.CoffeeMachine.Application.Queries.IBeverageQueries.IBeverageCollectionQueries
{
    /// <summary>
    /// 饮品集合查询接口
    /// </summary>
    public interface IBeverageCollectionInfoQueries
    {
        /// <summary>
        /// 获取饮品集合
        /// </summary>
        /// <returns></returns>
        Task<PagedResultDto<BeverageCollectionDto>> GetBeverageColltionListAsync(QueryRequest request, long enterpriseInfoId);

        /// <summary>
        /// 获取平台端饮品集合下拉数据
        /// </summary>
        /// <returns></returns>
        Task<List<P_BeverageCollectionSelectedDto>> GetP_BeverageCollectionsAsync();
    }
}