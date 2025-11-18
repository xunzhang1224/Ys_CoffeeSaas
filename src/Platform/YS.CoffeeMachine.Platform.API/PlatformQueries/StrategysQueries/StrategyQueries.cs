using System.Collections.Generic;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Dtos.TermServiceDtos;
using YS.CoffeeMachine.Application.PlatformDto.StrategyDtos;
using YS.CoffeeMachine.Application.PlatformQueries.StrategyQueries;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Platform.API.Queries;
using YS.CoffeeMachine.Platform.API.Queries.Pagings;

namespace YS.CoffeeMachine.Platform.API.PlatformQueries.StrategyQueries
{
    /// <summary>
    /// 获取地区查询
    /// </summary>
    public class StrategyQueries(CoffeeMachinePlatformDbContext context, IMapper mapper) : IStrategyQueries
    {
        /// <summary>
        /// 获取地区关联分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<AreaRelationDto>> GetAreaRelationList(QueryRequest request)
        {
            var list = await context.AreaRelation.AsQueryable()
                .Select(a => new AreaRelationDto
                {
                    Id = a.Id,
                    Area = a.Area,
                    Country = a.Country,
                    AreaCode = a.AreaCode,
                    Language = a.Language,
                    CurrencyId = a.CurrencyId,
                    TimeZone = a.TimeZone,
                    TermServiceUrl = a.TermServiceUrl,
                    Enabled = a.Enabled,
                    CreateTime = a.CreateTime,
                }).ToPagedListAsync(request);

            var termIds = ExtractLongValues(list.Items.Select(a => a.TermServiceUrl).Distinct().ToList());
            var termDics = context.TermServiceEntity.AsQueryable()
                .Where(a => termIds.Contains(a.Id))
                .ToDictionary(a => a.Id, a => a.Title);

            // 获取字典的key
            var dicKeys = list.Items.SelectMany(a => new[] { a.Area, a.Country, a.Language, a.TimeZone })
            .Distinct().ToList();
            var dics = context.Dictionary.AsQueryable().Where(a => dicKeys.Contains(a.Key)).ToDictionary(a => a.Key, a => a.Value);

            // 获取币种信息
            var currencyIds = list.Items.Select(a => a.CurrencyId).Distinct().ToList();
            var currencyDics = context.Currency.AsQueryable().Where(a => currencyIds.Contains(a.Id)).ToDictionary(a => a.Id, a => new { a.Code, a.Name });

            // 获取语言信息
            var languageKeys = list.Items.Select(a => a.Language).Distinct().ToList();
            var languageDics = context.LanguageInfo.AsQueryable().Where(a => languageKeys.Contains(a.Code)).ToDictionary(a => a.Code, a => a.Name);

            foreach (var item in list.Items)
            {
                item.AreaName = dics[item.Area];
                item.CountryName = dics[item.Country];
                item.LanguageName = languageDics[item.Language]; //dics[item.Language];
                item.TimeZoneName = dics[item.TimeZone];
                item.CurrencyCode = currencyDics[item.CurrencyId].Code;
                item.CurrencyName = currencyDics[item.CurrencyId].Name;
                // item.TermName = long.TryParse(item.TermServiceUrl, out long termServiceId) ? termDics.ContainsKey(termServiceId) ? termDics[termServiceId] : string.Empty : string.Empty;
                item.TermName = long.TryParse(item.TermServiceUrl, out var id) && termDics.TryGetValue(id, out string? name) ? name : string.Empty;
            }
            return list;
        }

        /// <summary>
        /// Array中提取long类型值
        /// </summary>
        /// <param name="inputList"></param>
        /// <returns></returns>
        public List<long> ExtractLongValues(List<string> inputList)
        {
            List<long> result = new List<long>();

            if (inputList == null) return result;

            foreach (string item in inputList)
            {
                if (!string.IsNullOrWhiteSpace(item) &&
                    long.TryParse(item.Trim(), out long longValue))
                {
                    result.Add(longValue);
                }
            }

            return result;
        }

        /// <summary>
        /// 企业所需地区关联列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<AreaRelationDto>> GetAreaRelationAllList()
        {
            var list = await context.AreaRelation.AsQueryable()
                .Where(a => a.Enabled == YS.CoffeeMachine.Domain.Shared.Enum.EnabledEnum.Enable)
                .Select(a => new AreaRelationDto
                {
                    Id = a.Id,
                    Country = a.Country,
                    TimeZone = a.TimeZone,
                }).ToListAsync();
            // 获取字典的key
            var dicKeys = list.SelectMany(a => new[] { a.Country, a.TimeZone })
                .Distinct().ToList();
            var dics = context.Dictionary.AsQueryable().Where(a => dicKeys.Contains(a.Key)).ToDictionary(a => a.Key, a => a.Value);
            foreach (var item in list)
            {
                item.CountryName = dics[item.Country];
                item.TimeZoneName = dics[item.TimeZone];
            }
            return list;
        }

        /// <summary>
        /// 获取服务条款分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<TermServiceOutput>> GetTermServicePageList(TermServiceInput request)
        {
            return await context.TermServiceEntity.AsQueryable()
                .WhereIf(!string.IsNullOrEmpty(request.Title), a => a.Title.Contains(request.Title))
                .Select(a => new TermServiceOutput
                {
                    Id = a.Id,
                    Title = a.Title,
                    Description = a.Description,
                    Enabled = a.Enabled,
                }).ToPagedListAsync(request);
        }

        /// <summary>
        /// 获取服务条款选择列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<TermServiceSelectOutput>> GetTermServiceSelectList()
        {
            return await context.TermServiceEntity.AsQueryable()
                .Where(a => a.Enabled == EnabledEnum.Enable)
                .Select(a => new TermServiceSelectOutput
                {
                    Id = a.Id,
                    Title = a.Title,
                }).ToListAsync();
        }

        /// <summary>
        /// 获取单个服务条款信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SingleTermServiceOutput> GetSingleTermServiceById(long id)
        {
            return await context.TermServiceEntity.AsQueryable()
                .Where(w => w.Id == id)
                .Select(a => new SingleTermServiceOutput
                {
                    Title = a.Title,
                    Content = a.Content,
                }).FirstOrDefaultAsync() ?? new SingleTermServiceOutput();
        }
    }
}
