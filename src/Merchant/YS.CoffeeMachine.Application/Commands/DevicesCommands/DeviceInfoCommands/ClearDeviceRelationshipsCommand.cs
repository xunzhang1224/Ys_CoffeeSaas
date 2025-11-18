using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceInfoCommands
{
    /// <summary>
    /// 清除设备关系命令
    /// </summary>
    public record ClearDeviceRelationshipsCommand(List<long> deviceId) : ICommand<bool>;
}