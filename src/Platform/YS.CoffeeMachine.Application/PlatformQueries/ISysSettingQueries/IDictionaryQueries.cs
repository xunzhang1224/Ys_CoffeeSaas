using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.SysSettingDtos;

namespace YS.CoffeeMachine.Application.PlatformQueries.ISysSettingQueries
{
    /// <summary>
    /// 字典查询
    /// </summary>
    public interface IDictionaryQueries
    {
        /// <summary>
        /// 获取语言字典列表
        /// </summary>
        /// <returns></returns>
        Task<List<SysDictionaryDto>> GetLanguageDicList();
    }
}
