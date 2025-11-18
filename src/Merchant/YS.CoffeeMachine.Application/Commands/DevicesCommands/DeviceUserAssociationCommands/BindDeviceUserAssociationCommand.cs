using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceUserAssociationCommands
{
    /// <summary>
    /// 设备绑定用户多对多
    /// </summary>
    /// <param name="deviceIds"></param>
    /// <param name="userIds"></param>
    public record BindDeviceUserAssociationCommand(List<long> deviceIds, List<long> userIds, long? enterpriseId) : ICommand<bool>;

    /// <summary>
    /// 用户绑定设备一对多
    /// </summary>
    /// <param name="userIds"></param>
    /// <param name="deviceIds"></param>
    public record UserBindDeviceCommand(long userId, List<long> deviceIds) : ICommand<bool>;
}
