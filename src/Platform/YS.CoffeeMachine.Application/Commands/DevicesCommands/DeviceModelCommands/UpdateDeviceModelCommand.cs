using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceModelCommands
{
    public record UpdateDeviceModelCommand(long id, string name, int maxCassetteCount, string remark) : ICommand<bool>;
}