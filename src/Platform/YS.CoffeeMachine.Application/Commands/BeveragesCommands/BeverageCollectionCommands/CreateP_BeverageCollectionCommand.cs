using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageCollectionCommands
{
    public record CreateP_BeverageCollectionCommand(string name, string languageKey, long deviceModelId, List<long> beverageInfoIds) : ICommand<bool>;
}
