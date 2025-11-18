using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceInfoCommands
{
    public record ActiveDeviceCommand(long deviceId) : ICommand;
}
