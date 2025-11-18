using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.PlatformDto.BeverageInfoDtos;

namespace YS.CoffeeMachine.Application.PlatformQueries.IBeverageQueries
{
    /// <summary>
    /// 饮品集合查询
    /// </summary>
    public interface IBeverageCollectionQueries
    {
        /// <summary>
        /// 获取饮品集合列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResultDto<P_BeverageCollectionDto>> GetBeverageCollectionList(P_BeverageCollectionInput request);
    }
}
