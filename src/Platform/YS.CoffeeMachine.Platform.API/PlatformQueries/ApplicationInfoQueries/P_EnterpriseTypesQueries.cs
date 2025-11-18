using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.PlatformDto.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.PlatformQueries.IApplicationInfoQueries;
using YS.CoffeeMachine.Infrastructure;

namespace YS.CoffeeMachine.Platform.API.PlatformQueries.ApplicationInfoQueries
{
    /// <summary>
    /// 平台端企业类型查询
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    public class P_EnterpriseTypesQueries(CoffeeMachinePlatformDbContext context, IMapper mapper) : IP_EnterpriseTypesQueries
    {
        /// <summary>
        /// 根据Id获取企业类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<P_EnterpriseTypesDto> GetEnterpriseTypeByIdAsync(long id)
        {
            var info = await context.EnterpriseTypes.FindAsync(id);
            return mapper.Map<P_EnterpriseTypesDto>(info);
        }

        /// <summary>
        /// 获取企业类型列表
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<P_EnterpriseTypesDto>> GetEnterpriseTypesAsync()
        {
            var list = await context.EnterpriseTypes.ToListAsync();
            return mapper.Map<List<P_EnterpriseTypesDto>>(list);
        }
    }
}
