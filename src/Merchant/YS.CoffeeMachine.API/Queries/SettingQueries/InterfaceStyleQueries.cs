using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Dtos.SettringDtos;
using YS.CoffeeMachine.Application.Queries.ISettingQueries;
using YS.CoffeeMachine.Infrastructure;

namespace YS.CoffeeMachine.API.Queries.SettingQueries
{
    /// <summary>
    /// 获取所有界面风格
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    public class InterfaceStyleQueries(CoffeeMachineDbContext context, IMapper mapper) : IInterfaceStyleQueries
    {
        /// <summary>
        /// 获取所有界面风格
        /// </summary>
        /// <returns></returns>
        public async Task<List<InterfaceStylesDto>> GetAllInterfaceStylesAsync()
        {
            var list = await context.InterfaceStyles.Where(w => !w.IsDelete).ToListAsync();
            return mapper.Map<List<InterfaceStylesDto>>(list);
        }
    }
}
