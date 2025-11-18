using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.PlatformDto.CurrencyDtos;

namespace YS.CoffeeMachine.Application.PlatformQueries.ICurrencyQueries
{
    /// <summary>
    /// 币种查询
    /// </summary>
    public interface ICurrencyInfoQueries
    {
        /// <summary>
        /// 币种查询
        /// </summary>
        /// <returns></returns>
        Task<List<CurrencyDto>> GetCurrencyList();
    }
}
