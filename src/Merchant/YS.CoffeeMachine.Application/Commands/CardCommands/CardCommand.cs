using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.CardCommands
{
    public record CreateCardCommand(string CardNumber) : ICommand<bool>;
    public record UpdateCommand(long CardId, string? CardNumber, bool? IsEnabled) : ICommand<bool>;
    public record CardBindDeviceCommand(List<long> DeviceIds, long CardId) : ICommand<bool>;
}
