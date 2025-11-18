using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.DevicesCommands.BindDeviceServiceProvidersCommands
{
    public record BindDeviceServiceProvidersCommand(long deviceId, List<long> spIds) : ICommand<bool>;
}
