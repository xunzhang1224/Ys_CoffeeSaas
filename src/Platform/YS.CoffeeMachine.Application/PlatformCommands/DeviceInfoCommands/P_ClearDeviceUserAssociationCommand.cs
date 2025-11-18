using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.DeviceInfoCommands
{
    public record P_ClearDeviceUserAssociationCommand(long deviceId) : ICommand<bool>;
}
