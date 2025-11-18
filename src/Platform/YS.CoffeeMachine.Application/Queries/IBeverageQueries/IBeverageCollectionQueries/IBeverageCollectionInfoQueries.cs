using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YS.CoffeeMachine.Application.Dtos.PagingDto;

namespace YS.CoffeeMachine.Application.Queries.IBeverageQueries.IBeverageCollectionQueries
{
    /// <summary>
    /// 饮品结合查询
    /// </summary>
    public interface IBeverageCollectionInfoQueries
    {
        /// <summary>
        /// 获取饮品集合
        /// </summary>
        /// <returns></returns>
        Task<PagedResultDto<BeverageCollectionDto>> GetBeverageColltionListAsync(QueryRequest request, long enterpriseInfoId);
    }
}
