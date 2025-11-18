using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;

namespace YS.CoffeeMachine.Application.Queries.IBeverageQueries
{
    /// <summary>
    /// 配方信息查询
    /// </summary>
    public interface IFormulaInfosQueries
    {
        /// <summary>
        /// 获取配方信息
        /// </summary>
        /// <returns></returns>
        Task<Dictionary<FormulaTypeEnum, string>> GetAllSpecsJson();
    }
}
