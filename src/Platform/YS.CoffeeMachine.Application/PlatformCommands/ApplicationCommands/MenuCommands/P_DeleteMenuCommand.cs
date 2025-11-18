using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.MenuCommands
{
    public record P_DeleteMenuCommand(long id) : ICommand<bool>;
}
