using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoCommands
{
    /// <summary>
    /// 批量修改设备下  饮品价格
    /// </summary>
    /// <param name="deviceId">设备id</param>
    /// <param name="beverageInfoPriceList">饮品价格集合</param>
    public record UpdateBeveragePriceCommand(long deviceId, List<BeverageInfoPriceInput> beverageInfoPriceList) : ICommand<bool>;
}
