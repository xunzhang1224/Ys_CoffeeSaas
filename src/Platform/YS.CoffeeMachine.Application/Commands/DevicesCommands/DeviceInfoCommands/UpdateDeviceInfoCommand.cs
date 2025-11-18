using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceInfoCommands
{
    public record UpdateDeviceInfoCommand(long id, string name, DevicePositionVoInfo devicePositionVo, UsageScenarioEnum usageScenario, List<long> groupIds, string pOSMachineNumber) : ICommand<bool>;
    public record SetOnLineCommand(string mid, bool OnLineStatus, int signal) : ICommand;
}
