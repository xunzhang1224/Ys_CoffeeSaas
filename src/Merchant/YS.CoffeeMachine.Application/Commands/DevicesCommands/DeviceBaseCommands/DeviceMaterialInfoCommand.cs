using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.DeviceDots;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceBaseCommands
{
    public record UpdateDeviceMaterialWarningCommand(List<UpdateDeviceWarningDto> dtos,long deviceBaseId) : ICommand<bool>;
}
