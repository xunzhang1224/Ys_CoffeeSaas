using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.SettingsCommands.InterfaceStylesCommands
{
    public record UpdateInterfaceStylesCommand(long id, string name, string code, string preview) : ICommand<bool>;
}
