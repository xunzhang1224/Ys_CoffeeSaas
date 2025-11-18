using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Dtos.SettringDtos;
using YS.CoffeeMachine.Application.Queries.ISettingQueries;
using YS.CoffeeMachine.Infrastructure;

namespace YS.CoffeeMachine.API.Queries.SettingQueries
{
    /// <summary>
    /// 获取所有时区信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    public class TimeZoneInfoQueries(CoffeeMachineDbContext context, IMapper mapper) : ITimeZoneInfoQueries
    {
        /// <summary>
        /// 获取所有时区信息
        /// </summary>
        public async Task<List<TimeZoneInfoDto>> GetAllTimeZoneInfoAsync()
        {
            var list = await context.TimeZoneInfos.Where(w => !w.IsDelete).ToListAsync();
            return mapper.Map<List<TimeZoneInfoDto>>(list);
        }
    }
}
