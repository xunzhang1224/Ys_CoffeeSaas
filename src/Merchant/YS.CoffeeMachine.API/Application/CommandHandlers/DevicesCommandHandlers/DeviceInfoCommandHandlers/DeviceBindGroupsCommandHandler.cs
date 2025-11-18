using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceInfoCommands;
using YS.CoffeeMachine.Domain.IRepositories.DevicesIRepositories;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.DevicesCommandHandlers.DeviceInfoCommandHandlers
{
    /// <summary>
    /// 设备绑定分组
    /// </summary>
    /// <param name="repository"></param>
    public class DeviceBindGroupsCommandHandler(IDeviceInfoRepository repository) : ICommandHandler<DeviceBindGroupsCommand, bool>
    {
        /// <summary>
        /// 设备绑定分组
        /// </summary>
        public async Task<bool> Handle(DeviceBindGroupsCommand request, CancellationToken cancellationToken)
        {
            var info = await repository.GetByIdAsync(request.deviceId);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0008)]);
            info.BindGroups(request.groupIds);
            var res = repository.UpdateAsync(info);
            return res != null;
        }
    }
}
