using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.Application.Dtos.BasicDtos.Language;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Dtos.SettringDtos;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Language;

namespace YS.CoffeeMachine.Application.Queries.BasicQueries.Language
{
    /// <summary>
    /// 语言信息查询接口，提供多语言相关的查询操作
    /// 包含语言列表获取、语言文本分页查询、语言字典获取等功能
    /// </summary>
    public interface ILanguageInfoQueries
    {
        /// <summary>
        /// 将语言信息列表转换为字典形式，以语言编码为键
        /// </summary>
        /// <param name="dto">语言信息 DTO 列表</param>
        /// <returns>语言字典，Key 为语言编码</returns>
        Task<Dictionary<string, LanguageInfoDto>> GetDicByLanguageInfo(List<LanguageInfoDto> dto);

        /// <summary>
        /// 获取所有语言的列表
        /// </summary>
        /// <returns>语言信息 DTO 列表</returns>
        Task<List<LanguageInfo>> GetAllLanguageAsync();

        /// <summary>
        /// 分页获取语言列表
        /// </summary>
        /// <param name="request">分页请求参数</param>
        /// <returns>分页后的语言信息 DTO 结果</returns>
        Task<PagedResultDto<LanguageInfoDto>> GetAllLanguagePageAsync([FromBody] QueryRequest request);

        /// <summary>
        /// 分页获取语言文本列表
        /// </summary>
        /// <param name="request">分页请求参数</param>
        /// <returns>分页后的语言文本 DTO 结果</returns>
        Task<PagedResultDto<LanguageTextDto>> GetLanguageTextPageAsync([FromBody] QueryRequest request);

        /// <summary>
        /// 根据语言文本编码获取语言文本详情
        /// </summary>
        /// <param name="code">语言文本编码（如：login.welcome）</param>
        /// <returns>语言文本详细 DTO</returns>
        Task<GetLanguageTextDetailDto> GetDetailAsync(string code);

        /// <summary>
        /// 获取指定语言的文本字典
        /// </summary>
        /// <param name="input">包含语言编码的输入参数</param>
        /// <returns>语言文本字典，Key 为文本编码，Value 为对应文本值</returns>
        Task<Dictionary<string, string>> GetLanguageTextsAsync(LanguageTextInput input);

        /// <summary>
        /// 获取当前上下文中的语言编码
        /// 优先级：请求上下文 > Redis 默认语言 > 数据库默认语言
        /// </summary>
        /// <returns>当前语言编码字符串</returns>
        Task<string> GetLangCodeAsync();

        /// <summary>
        /// 根据文本编码获取对应语言文本的值
        /// </summary>
        /// <param name="code">语言文本编码（如：login.title）</param>
        /// <returns>对应编码的语言文本值</returns>
        Task<string> GetLangTextByCodeAsync(string code);

        /// <summary>
        /// 获取全部语言文本字典
        /// </summary>
        /// <returns>完整的语言文本字典，Key 为文本编码，Value 为对应文本值</returns>
        Task<Dictionary<string, string>> GetLanguageTextsAsync(string langCode = null);
    }
}
