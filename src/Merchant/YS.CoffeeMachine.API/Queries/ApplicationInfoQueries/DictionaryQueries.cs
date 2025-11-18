using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.Queries.IApplicationInfoQueries;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;

namespace YS.CoffeeMachine.API.Queries.ApplicationInfoQueries
{
    /// <summary>
    /// 字典查询
    /// </summary>
    /// <param name="coffeeMachineDb"></param>
    public class DictionaryQueries(CoffeeMachineDbContext coffeeMachineDb): IDictionaryQueries
    {
        /// <summary>
        /// 获取字典附表
        /// </summary>
        /// <param name="parentKey"></param>
        /// <returns></returns>
        public async Task<List<DicionaryUseDto>> GetDictionarySubAsync(string parentKey)
        {
            return await coffeeMachineDb.Dictionary.AsQueryable()
               .Where(a => a.ParentKey == parentKey && a.IsEnabled == EnabledEnum.Enable)
               .Select(a => new DicionaryUseDto
               {
                   Key = a.Key,
                   Value = a.Value,
                   IsEnabled = a.IsEnabled
               })
               .ToListAsync();
        }
    }
}
