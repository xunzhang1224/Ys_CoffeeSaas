using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceInfoCommands
{
    public record CreateDeviceBaseInfoCommand(string Mid, string MachineStickerCode, string BoxId,string ModelName) : ICommand<string>;
    public record UnBindCommand(string Mid) : ICommand<bool>;
}
