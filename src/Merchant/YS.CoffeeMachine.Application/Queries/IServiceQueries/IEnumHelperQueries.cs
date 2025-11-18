using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.IServices;

namespace YS.CoffeeMachine.Application.Queries.IServiceQueries
{
    /// <summary>
    /// 枚举帮助类
    /// </summary>
    public interface IEnumHelperQueries
    {
        /// <summary>
        /// 获取枚举信息
        /// </summary>
        /// <returns></returns>
        Task<List<EnumInfo>> GetAllEnumInfos();
    }
}
