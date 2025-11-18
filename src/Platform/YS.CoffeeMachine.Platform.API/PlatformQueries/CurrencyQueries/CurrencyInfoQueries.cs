using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.PlatformDto.CurrencyDtos;
using YS.CoffeeMachine.Application.PlatformQueries.ICurrencyQueries;
using YS.CoffeeMachine.Infrastructure;

namespace YS.CoffeeMachine.Platform.API.PlatformQueries.CurrencyQueries
{
    /// <summary>
    /// 币种查询
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    public class CurrencyInfoQueries(CoffeeMachinePlatformDbContext context, IMapper mapper) : ICurrencyInfoQueries
    {
        /// <summary>
        /// 获取币种
        /// </summary>
        /// <returns></returns>
        public async Task<List<CurrencyDto>> GetCurrencyList()
        {
            return await context.Currency.AsQueryable()
                 .Select(a => new CurrencyDto
                 {
                     Id = a.Id,
                     Code = a.Code,
                     Name = a.Name,
                     CurrencySymbol = a.CurrencySymbol,
                     CurrencyShowFormat = a.CurrencyShowFormat,
                     Accuracy = a.Accuracy,
                     RoundingType = a.RoundingType,
                     Enabled = a.Enabled,
                     CreateTime = a.CreateTime
                 }).ToListAsync();
        }
    }
}
