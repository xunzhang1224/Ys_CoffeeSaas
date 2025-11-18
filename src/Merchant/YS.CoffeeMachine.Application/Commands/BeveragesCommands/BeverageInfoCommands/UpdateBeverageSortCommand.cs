using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoCommands
{
    public record UpdateBeverageSortCommand(long deviceId, List<BeverageInfoSortInput> beverageInfoSortList) : ICommand<bool>;
}
