using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;

namespace YS.CoffeeMachine.Application.Queries.IApplicationInfoQueries
{
    /// <summary>
    /// 字典查询
    /// </summary>
    public interface IDictionaryQueries
    {
        /// <summary>
        /// 获取字典详细
        /// </summary>
        /// <param name="parentKey">父级key</param>
        /// <returns>字典集合</returns>
        Task<List<DicionaryUseDto>> GetDictionarySubAsync(string parentKey);
    }
}
