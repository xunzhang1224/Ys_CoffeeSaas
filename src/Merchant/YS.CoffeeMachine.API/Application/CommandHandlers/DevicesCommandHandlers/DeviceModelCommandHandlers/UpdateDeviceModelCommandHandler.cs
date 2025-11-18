using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceModelCommands;
using YS.CoffeeMachine.Domain.IRepositories.DevicesIRepositories;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.DevicesCommandHandlers.DeviceModelCommandHandlers
{
    /// <summary>
    /// 更新设备型号
    /// </summary>
    /// <param name="repository"></param>
    public class UpdateDeviceModelCommandHandler(IDeviceModelRepository repository) : ICommandHandler<UpdateDeviceModelCommand, bool>
    {
        /// <summary>
        /// 更新设备型号
        /// </summary>
        public async Task<bool> Handle(UpdateDeviceModelCommand request, CancellationToken cancellationToken)
        {
            var info = await repository.GetAsync(request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            info.Update(request.name, request.maxCassetteCount, request.remark);
            var res = await repository.UpdateAsync(info);
            return res != null;
        }
    }
}
