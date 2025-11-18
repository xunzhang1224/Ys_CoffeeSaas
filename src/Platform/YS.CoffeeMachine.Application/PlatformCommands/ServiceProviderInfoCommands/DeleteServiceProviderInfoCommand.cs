using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.ServiceProviderInfoCommands
{
    public record DeleteServiceProviderInfoCommand(long id) : ICommand<bool>;
}
