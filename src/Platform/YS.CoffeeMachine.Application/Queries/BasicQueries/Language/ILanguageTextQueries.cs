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
    /// 语言文本查询
    /// </summary>
    public interface ILanguageTextQueries
    {
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<GetLanguageTextDetailDto> GetDetail(string code);

        /// <summary>
        /// 获取语言文本分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResultDto<LanguageTextDto>> GetLanguageTextPage([FromBody] QueryRequest request);

        /// <summary>
        /// 获取语言文本
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Dictionary<string,string>> GetLanguageTexts(LanguageTextInput input);
    }
}
