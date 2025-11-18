using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.PlatformDto.BasicDtos;
using YS.CoffeeMachine.Application.Queries.BasicQueries.Language;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Dictionary;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Platform.API.Queries.Pagings;

namespace YS.CoffeeMachine.Platform.API.Queries.Dictionary
{
    /// <summary>
    /// 字典查询
    /// </summary>
    /// <param name="coffeeMachineDb"></param>
    public class DictionaryQueries(CoffeeMachinePlatformDbContext coffeeMachineDb) : IDictionaryQueries
    {
        /// <summary>
        /// 获取字典根据key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<DictionaryEntity>> GetDictionaryByKey(string key = null, string parentKey = null)
        {
            var dicEf = coffeeMachineDb.DictionaryEntity;
            if (!string.IsNullOrWhiteSpace(key))
            {
                dicEf.Where(x => x.Key.Contains(key));
            }
            if (string.IsNullOrWhiteSpace(parentKey))
            {
                dicEf.Where(x => string.IsNullOrWhiteSpace(x.ParentKey));
            }
            return await dicEf.ToListAsync();
        }

        /// <summary>
        /// 获取字典根据key
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<DictionaryDto>> GetDictionaryByKey(DictionaryInput req)
        {
            return await coffeeMachineDb.Dictionary.AsQueryable().AsNoTracking()
             .WhereIf(!string.IsNullOrWhiteSpace(req.Key), a => a.Key.Contains(req.Key))
             .WhereIf(!string.IsNullOrWhiteSpace(req.Name), a => a.Value.Contains(req.Name))
             .Where(a => a.ParentKey == null)
             .Select(a => new DictionaryDto()
             {
                 Key = a.Key,
                 Value = a.Value,
                 ParentKey = a.ParentKey,
                 IsEnabled = a.IsEnabled

             }).ToPagedListAsync(req);
        }

        /// <summary>
        /// 获取字典附表，只查启用的字典
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
                   IsEnabled = a.IsEnabled,
                   Remark = a.Remark
               })
               .ToListAsync();
        }

        /// <summary>
        /// 根据父级Key获取字典列表，包含禁用
        /// </summary>
        /// <param name="parentKey"></param>
        /// <returns></returns>
        public async Task<List<DicionaryUseDto>> GetDictionarySubContainDisableAsync(string parentKey)
        {
            return await coffeeMachineDb.Dictionary.AsQueryable()
               .Where(a => a.ParentKey == parentKey)
               .Select(a => new DicionaryUseDto
               {
                   Key = a.Key,
                   Value = a.Value,
                   IsEnabled = a.IsEnabled
               })
               .ToListAsync();
        }

        /// <summary>
        /// 获取地区关联所需的字典list
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictionaryDto>> GetAreaRelationNeedDictionaryAsync()
        {
            return await coffeeMachineDb.Dictionary.AsQueryable()
                .Where(a => (a.ParentKey == "language" || a.ParentKey == "Area" || a.ParentKey == "Country" || a.ParentKey == "TimeZone") && a.IsEnabled == EnabledEnum.Enable)
                 .Select(a => new DictionaryDto()
                 {
                     Key = a.Key,
                     Value = a.Value,
                     ParentKey = a.ParentKey,
                     IsEnabled = a.IsEnabled

                 }).ToListAsync();
        }

        /// <summary>
        /// 获取语言字典列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<DicionaryUseDto>> GetLanguageDicList()
        {
            return await coffeeMachineDb.Dictionary.AsQueryable()
                .Where(a => a.ParentKey == "language" && a.IsEnabled == EnabledEnum.Enable)
                .Select(a => new DicionaryUseDto
                {
                    Key = a.Key,
                    Value = a.Value,
                    IsEnabled = a.IsEnabled
                })
                .ToListAsync();
        }

        /// <summary>
        /// 根据父级key的数组返回对应
        /// </summary>
        /// <param name="parentKeys">父级keys</param>
        /// <returns></returns>
        public async Task<List<DictionaryDto>> GetDictionaryByArry(List<string> parentKeys)
        {
            return await coffeeMachineDb.Dictionary.AsQueryable()
                .WhereIf(parentKeys != null && parentKeys.Count > 0, a => parentKeys!.Contains(a.ParentKey))
                .Where(w => w.IsEnabled == EnabledEnum.Enable)
                 .Select(a => new DictionaryDto()
                 {
                     Key = a.Key,
                     Value = a.Value,
                     ParentKey = a.ParentKey,
                     IsEnabled = a.IsEnabled,
                     Remark = a.Remark
                 }).ToListAsync();
        }

        /// <summary>
        /// 获取地区字典列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<DicionaryUseDto>> GetAreaDicList()
        {
            return await coffeeMachineDb.Dictionary.AsQueryable()
                .Where(a => a.ParentKey == "Area" && a.IsEnabled == EnabledEnum.Enable)
                .Select(a => new DicionaryUseDto
                {
                    Key = a.Key,
                    Value = a.Value
                })
                .ToListAsync();
        }

        /// <summary>
        /// 获取国家字典列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<DicionaryUseDto>> GetCountryDicList()
        {
            return await coffeeMachineDb.Dictionary.AsQueryable()
                .Where(a => a.ParentKey == "Country" && a.IsEnabled == EnabledEnum.Enable)
                .Select(a => new DicionaryUseDto
                {
                    Key = a.Key,
                    Value = a.Value
                })
                .ToListAsync();
        }

        /// <summary>
        /// 获取时区字典列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<DicionaryUseDto>> GetTimeZoneDicList()
        {
            return await coffeeMachineDb.Dictionary.AsQueryable()
                .Where(a => a.ParentKey == "TimeZone" && a.IsEnabled == EnabledEnum.Enable)
                .Select(a => new DicionaryUseDto
                {
                    Key = a.Key,
                    Value = a.Value
                })
                .ToListAsync();
        }
    }
}
