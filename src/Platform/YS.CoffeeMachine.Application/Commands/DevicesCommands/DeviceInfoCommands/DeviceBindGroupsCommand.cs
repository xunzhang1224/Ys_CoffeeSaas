using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceInfoCommands
{
    public record DeviceBindGroupsCommand(long deviceId, List<long> groupIds) : ICommand<bool>;
}
