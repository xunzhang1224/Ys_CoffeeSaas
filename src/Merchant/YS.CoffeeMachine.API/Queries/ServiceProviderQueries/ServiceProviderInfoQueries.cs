using AutoMapper;
using YS.CoffeeMachine.API.Queries.Pagings;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Dtos.ServiceProviderDtos;
using YS.CoffeeMachine.Application.Queries.IServiceProviderInfoQueries;
using YS.CoffeeMachine.Infrastructure;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.API.Queries.ServiceProviderQueries
{
    /// <summary>
    /// 服务商查询
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    public class ServiceProviderInfoQueries(CoffeeMachineDbContext context, IMapper mapper) : IServiceProviderInfoQueries
    {
        /// <summary>
        /// 获取服务商分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ServiceProviderInfoDto>> GetServiceProviderInfoListAsync(QueryRequest request)
        {
            var list = await context.ServiceProviderInfo.ToPagedListAsync(request);
            var listDto = list.Items.Select(s => mapper.Map<ServiceProviderInfoDto>(s));
            PagedResultDto<ServiceProviderInfoDto> pagedResultDto = new PagedResultDto<ServiceProviderInfoDto>()
            {
                Items = listDto.ToList(),
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = list.TotalCount
            };
            return pagedResultDto;
        }
    }
}
