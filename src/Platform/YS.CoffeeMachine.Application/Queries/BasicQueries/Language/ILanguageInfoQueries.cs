using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.Application.Dtos.BasicDtos.Language;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Dtos.SettringDtos;

namespace YS.CoffeeMachine.Application.Queries.BasicQueries.Language
{
    /// <summary>
    /// 语言查询
    /// </summary>
    public interface ILanguageInfoQueries
    {
        /// <summary>
        /// 获取语言
        /// </summary>
        /// <param name="langCode"></param>
        /// <returns></returns>
        Task<LanguageInfoDto> GetLanguageAsync(string langCode);
        /// <summary>
        /// 转字典
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<Dictionary<string, LanguageInfoDto>> GetDicByLanguageInfo(List<LanguageInfoDto> dto);

        /// <summary>
        /// 获取所有语言
        /// </summary>
        /// <returns></returns>
        Task<List<LanguageInfoDto>> GetAllLanguageAsync();

        /// <summary>
        /// 获取所有语言分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResultDto<LanguageInfoDto>> GetAllLanguagePageAsync(QueryRequest request);

        /// <summary>
        /// 获取语言文本分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResultDto<LanguageTextDto>> GetLanguageTextPageAsync(LanguageTextQuery request);

        /// <summary>
        /// 根据语言文本编码获取语言文本详情
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<GetLanguageTextDetailDto> GetDetailAsync(string code);

        /// <summary>
        /// 获取语言文本列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Dictionary<string, string>> GetLanguageTextsAsync(LanguageTextInput input);

        /// <summary>
        /// 获取语种code
        /// 请求上下文 > redis默认语种 > 数据库默认语种
        /// </summary>
        /// <returns></returns>
        Task<string> GetLangCodeAsync();

        /// <summary>
        /// 获取语言文本值
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<string> GetLangTextByCodeAsync(string code);

        /// <summary>
        /// 获取语言字典
        /// </summary>
        /// <returns></returns>
        Task<Dictionary<string, string>> GetLanguageTextsAsync(string langCode = null);
    }
}
