using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceUserAssociationCommands
{
    public record BindDeviceUserAssociationCommand(List<long> deviceIds, List<long> userIds) : ICommand<bool>;
}
