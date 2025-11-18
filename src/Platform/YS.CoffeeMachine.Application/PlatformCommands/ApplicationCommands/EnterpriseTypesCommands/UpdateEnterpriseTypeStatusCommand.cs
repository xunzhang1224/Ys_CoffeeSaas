using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.EnterpriseTypesCommands
{
    public record UpdateEnterpriseTypeStatusCommand(long Id, bool Status) : ICommand<bool>;
}
