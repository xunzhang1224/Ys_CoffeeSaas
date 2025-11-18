using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Dtos.SysSettingDtos;
using YS.CoffeeMachine.Application.PlatformQueries.ISysSettingQueries;
using YS.CoffeeMachine.Infrastructure;

namespace YS.CoffeeMachine.Platform.API.PlatformQueries.SysSettingQueries
{
    /// <summary>
    /// 字典查询
    /// </summary>
    /// <param name="context"></param>
    public class DictionaryQueries(CoffeeMachinePlatformDbContext context) : IDictionaryQueries
    {
        /// <summary>
        /// 获取语言字典列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<SysDictionaryDto>> GetLanguageDicList()
        {
            return await context.DictionaryEntity.AsQueryable()
                .Where(a => a.ParentKey == "language")
                .Select(a => new SysDictionaryDto
                {
                    Code = a.Key,
                    Value = a.Value
                })
                .ToListAsync();
        }
    }
}
