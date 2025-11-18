using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.CurrencyCommands
{
    /// <summary>
    /// 修改币种
    /// </summary>
    /// <param name="id">币种id</param>
    /// <param name="currencySymbol">货币名称</param>
    /// <param name="currencyShowFormat">货币符号</param>
    /// <param name="enabled">是否启用</param>
    public record UpdateCurrencyCommand(long id, string name, string currencySymbol, CurrencyShowFormatEnum currencyShowFormat, EnabledEnum enabled) : ICommand<bool>;
}
