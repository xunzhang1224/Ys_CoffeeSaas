using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.DeviceInfoCommands
{
    public record P_EnterpriseDeviceAllocationCommand(long deviceBaseId, long? deviceId, long enterpriseId, bool isClearRelationship = false) : ICommand<bool>;

    public record DeviceUnbindEnterpriseCommand(long deviceId, long enterpriseId, bool isClearRelationship = false) : ICommand;
}