using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.DevicesCommands.EnterpriseDevicesCommand
{
    public record CreateEnterpriseDevicesCommand(List<long> deviceIds, long enterpriseId, DeviceAllocationEnum allocationEnum, DateTime? recyclingTime, bool isClearRelationship = false) : ICommand<bool>;
}