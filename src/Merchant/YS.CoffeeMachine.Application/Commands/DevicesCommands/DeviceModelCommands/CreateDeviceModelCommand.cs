using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceModelCommands
{
    public record CreateDeviceModelCommand(string key, string name, int maxCassetteCount, string remark, string type) : ICommand<bool>;
}
