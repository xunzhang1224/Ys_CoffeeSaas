using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.StrategyCommands.AreaRelationCommands
{
    public record DeleteAreaRelationCommand(long id) : ICommand<bool>;
}
