using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceInfoCommands
{
    public record DeviceAttributeReportCommand(Dictionary<string,string> telemetrys, Dictionary<string, string> properties,string mid) : ICommand;
}
