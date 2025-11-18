using AutoMapper;
using FreeRedis;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using System.Globalization;
using YS.CoffeeMachine.Application.Dtos.BasicDtos.Language;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Dtos.SettringDtos;
using YS.CoffeeMachine.Application.Queries.BasicQueries.Language;
using YS.CoffeeMachine.Domain.IPlatformRepositories.Basics.Language;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YS.CoffeeMachine.Platform.API.Queries;
using YS.CoffeeMachine.Platform.API.Queries.Pagings;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.PlatformQueries.BasicQueries.Language
{
    /// <summary>
    /// 查询语言信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <param name="_redisClient"></param>
    /// <param name="_repository"></param>
    public class LanguageInfoQueries(CoffeeMachinePlatformDbContext context,
        IMapper mapper,
        IRedisClient _redisClient,
        IPLanguageInfoRepository _repository) : ILanguageInfoQueries
    {
        /// <summary>
        /// 获取语言文本分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<LanguageInfoDto>> GetAllLanguagePageAsync(QueryRequest request)
        {
            var list = await context.LanguageInfo.AsNoTracking().ToPagedListAsync(request);
            return mapper.Map<PagedResultDto<LanguageInfoDto>>(list);
        }

        /// <summary>
        /// 获取所有语言类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<LanguageInfoDto>> GetAllLanguageAsync()
        {
            var list = await context.LanguageInfo.AsNoTracking().ToListAsync();

            if (list == null || list.Count == 0)
            {
                return new List<LanguageInfoDto>();
            }
            var result = mapper.Map<List<LanguageInfoDto>>(list);
            return result;
        }

        /// <summary>
        /// 获取指定语言类型
        /// </summary>
        /// <param name="langCode"></param>
        /// <returns></returns>
        public async Task<LanguageInfoDto> GetLanguageAsync(string langCode)
        {
            var cachekey = string.Format(CacheConst.MultilingualAll);
            var redisLanguageInfoDto = await _redisClient.HGetAsync<LanguageInfoDto>(CacheConst.MultilingualAll, langCode);
            if (redisLanguageInfoDto == null)
            {
                var data = await context.LanguageInfo.AsNoTracking().Where(x => x.Code == langCode).FirstOrDefaultAsync();
                if (data == null)
                {
                    throw ExceptionHelper.AppFriendly(string.Format(L.Text[nameof(ErrorCodeEnum.D0002)], langCode));
                }
                await _redisClient.HSetAsync(cachekey, langCode, data);
                redisLanguageInfoDto = mapper.Map<LanguageInfoDto>(data);
            }
            return redisLanguageInfoDto;
        }

        /// <summary>
        /// GetDicByLanguageInfo
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, LanguageInfoDto>> GetDicByLanguageInfo(List<LanguageInfoDto> dto)
        {
            var dic = new Dictionary<string, LanguageInfoDto>();
            foreach (var item in dto)
            {
                dic.Add(item.Code, mapper.Map<LanguageInfoDto>(item));
            }
            return dic;
        }
        private IEnumerable<LanguageInfoDto> GetLanguageInfoDtos(Dictionary<string, LanguageInfoDto> dics)
        {
            foreach (var item in dics)
            {
                yield return item.Value;
            }
        }

        /// <summary>
        /// 获取语言文本分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<LanguageTextDto>> GetLanguageTextPageAsync(LanguageTextQuery request)
        {
            var list = await context.LanguageText.AsNoTracking().Include(x => x.Lang)
                .WhereIf(!string.IsNullOrEmpty(request.Code), w => w.Code == request.Code)
                .WhereIf(!string.IsNullOrEmpty(request.LangCode), w => w.Lang.Code == request.LangCode)
                .WhereIf(!string.IsNullOrEmpty(request.Text), w => w.Value.Contains(request.Text))
                .ToPagedListAsync(request);
            //var list = await context.LanguageText.ToPagedListAsync(request, "Language");
            return mapper.Map<PagedResultDto<LanguageTextDto>>(list);
        }
        /// <summary>
        /// 根据语言文本编码获取语言文本详情
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<GetLanguageTextDetailDto> GetDetailAsync(string code)
        {
            var languageTexts = await _repository.GetLanguageInfosByTextCodeAsync(code);
            if (languageTexts == null || languageTexts.Count == 0)
            {
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0004)]);
            }
            #region 组装返回内容
            List<LanguageTextItem> languageTextItems = new List<LanguageTextItem>();
            foreach (var item in languageTexts)
            {
                languageTextItems.Add(new LanguageTextItem()
                {
                    LangCode = item.Code,
                    LangName = item.Name,
                    Value = item.LanguageTextEntitys.FirstOrDefault()?.Value
                });
            }

            var ls_LanguageText = languageTexts[0];
            return new GetLanguageTextDetailDto()
            {
                Code = ls_LanguageText.Code,
                LangList = languageTextItems
            };
            #endregion
        }

        /// <summary>
        /// 获取语言文本列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> GetLanguageTextsAsync(LanguageTextInput input)
        {
            return await GetLanguageTextsAsync(input.LangCode);
        }

        /// <summary>
        /// 获取语种
        /// 未获取到请求上下文，取默认语种
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetLangCodeAsync()
        {
            var cul = CultureInfo.CurrentCulture.Name;
            if (string.IsNullOrWhiteSpace(cul))
            {
                cul = _repository.GetDefaultLanguageCode();
            }
            return cul;
        }

        /// <summary>
        /// 获取指定语种语言文本根据code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<string> GetLangTextByCodeAsync(string code)
        {
            var langCode = await GetLangCodeAsync();
            string result = string.Empty;
            var cachekey = CacheConst.MultilingualAll;
            var languageTexts = await _redisClient.HGetAsync<Dictionary<string, string>>(cachekey, langCode) ?? new Dictionary<string, string>();
            if (languageTexts == null || !languageTexts.Keys.Contains(code))
            {
                var info = await _repository.GetLanguageInfosByTextCodeAsync(code, langCode);
                result = info?.FirstOrDefault()?.LanguageTextEntitys?.FirstOrDefault()?.Value;
                if (result == null)
                    result = code;
                else
                {
                    var dic = new Dictionary<string, Dictionary<string, string>>();
                    var datas = await _repository.GetLanguageInfoAsync();
                    foreach (var data in datas)
                    {
                        var d = new Dictionary<string, string>();
                        foreach (var item in data.LanguageTextEntitys)
                        {
                            d.Add(item.Code, item.Value);
                        }
                        dic.Add(data.Code, d);
                    }
                    await _redisClient.HSetAsync(cachekey, dic);
                    await _redisClient.ExpireAsync(cachekey, TimeSpan.FromDays(7));
                }
            }
            else
            {
                result = languageTexts[code];
            }

            return result;
        }

        /// <summary>
        /// 获取指定语种全部语言文本
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> GetLanguageTextsAsync(string langCode = null)
        {
            if (langCode == null)
                langCode = await GetLangCodeAsync();
            var cachekey = CacheConst.MultilingualAll;
            var langTexts = await _redisClient.HGetAsync<Dictionary<string, string>>(cachekey, langCode)??new Dictionary<string, string>();
            if (langTexts == null || langTexts.Count == 0)
            {
                var lang = await _repository.GetLanguageInfoByCodeAsync(langCode);
                if (lang == null || lang.LanguageTextEntitys == null || lang.LanguageTextEntitys.Count == 0)
                    return new Dictionary<string, string>();
                else
                {
                    var dic = new Dictionary<string, Dictionary<string, string>>();
                    var datas = await _repository.GetLanguageInfoAsync();
                    foreach (var data in datas)
                    {
                        var d = new Dictionary<string, string>();
                        foreach (var item in data.LanguageTextEntitys)
                        {
                            d[item.Code] = item.Value;
                        }
                        dic[data.Code] = d;
                        //dic.Add(data.Code, d);
                    }
                    lang.LanguageTextEntitys.ToList().ForEach(x => langTexts.Add(x.Code, x.Value));
                    await _redisClient.HSetAsync(cachekey, dic);
                    await _redisClient.ExpireAsync(cachekey, TimeSpan.FromDays(7));
                }
            }
            return langTexts;
        }
    }
}
