using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;

namespace YS.CoffeeMachine.Application.Queries.IBeverageQueries
{
    /// <summary>
    /// 物料查询
    /// </summary>
    public interface IFormulaInfosQueries
    {
        /// <summary>
        /// 获取所有specs
        /// </summary>
        /// <returns></returns>
        Task<Dictionary<FormulaTypeEnum, string>> GetAllSpecsJson();
    }
}
