using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.BasicDtos.Language;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Dtos.SettringDtos;

namespace YS.CoffeeMachine.Application.Queries.BasicQueries.Language
{
    /// <summary>
    /// 语言文本查询接口，用于获取多语言文本信息
    /// 支持根据编码获取详情、分页查询语言文本、获取语言文本字典等操作
    /// </summary>
    public interface ILanguageTextQueries
    {
        /// <summary>
        /// 根据语言文本编码获取语言文本详情
        /// </summary>
        /// <param name="code">语言文本编码（如：login.title、home.welcome）</param>
        /// <returns>语言文本详细信息 DTO 对象</returns>
        Task<GetLanguageTextDetailDto> GetDetail(string code);

        /// <summary>
        /// 分页获取语言文本列表
        /// </summary>
        /// <param name="request">分页请求参数</param>
        /// <returns>分页后的语言文本 DTO 结果</returns>
        Task<PagedResultDto<LanguageTextDto>> GetLanguageTextPage([FromBody] QueryRequest request);

        /// <summary>
        /// 获取指定语言的文本字典
        /// </summary>
        /// <param name="input">包含语言编码的输入参数</param>
        /// <returns>语言文本字典，Key 为文本编码，Value 为对应文本值</returns>
        Task<Dictionary<string, string>> GetLanguageTexts(LanguageTextInput input);
    }
}
