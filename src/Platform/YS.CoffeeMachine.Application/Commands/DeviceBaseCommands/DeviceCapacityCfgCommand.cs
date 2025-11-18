using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.DeviceBaseCommands
{
    public record UpdateDeviceCapacityCfgCommand(Dictionary<int, string> dic, long deviceBaseId) : ICommand<bool>;
}
