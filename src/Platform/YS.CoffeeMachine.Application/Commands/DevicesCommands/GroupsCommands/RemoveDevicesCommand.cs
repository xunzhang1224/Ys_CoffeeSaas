using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.DevicesCommands.GroupsCommands
{
    public record RemoveDevicesCommand(long groupId, List<long> deviceIds) : ICommand<bool>;
}
