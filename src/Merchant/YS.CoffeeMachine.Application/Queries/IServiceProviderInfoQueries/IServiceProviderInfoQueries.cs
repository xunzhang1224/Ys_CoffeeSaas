using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Dtos.ServiceProviderDtos;

namespace YS.CoffeeMachine.Application.Queries.IServiceProviderInfoQueries
{
    /// <summary>
    /// 服务商信息查询接口
    /// </summary>
    public interface IServiceProviderInfoQueries
    {
        /// <summary>
        /// 获取服务商分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResultDto<ServiceProviderInfoDto>> GetServiceProviderInfoListAsync(QueryRequest request);
    }
}
