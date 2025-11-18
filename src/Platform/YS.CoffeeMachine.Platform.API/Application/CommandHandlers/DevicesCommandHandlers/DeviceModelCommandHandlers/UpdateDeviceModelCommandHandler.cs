using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceModelCommands;
using YS.CoffeeMachine.Domain.IPlatformRepositories.DevicesIRepositories;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.DevicesCommandHandlers.DeviceModelCommandHandlers
{
    /// <summary>
    /// 更新设备型号
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="context"></param>
    public class UpdateDeviceModelCommandHandler(IPDeviceModelRepository repository, CoffeeMachinePlatformDbContext context) : ICommandHandler<UpdateDeviceModelCommand, bool>
    {
        /// <summary>
        /// 更新设备型号
        /// </summary>
        public async Task<bool> Handle(UpdateDeviceModelCommand request, CancellationToken cancellationToken)
        {
            // 验证设备型号名是否存在
            var verify = await context.DeviceModel.AnyAsync(w => w.Name == request.name && w.Id != request.id);
            if (verify)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0012)]);

            var info = await repository.GetAsync(request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

            info.Update(request.name, request.maxCassetteCount, request.remark);
            var res = await repository.UpdateAsync(info);
            return res != null;
        }
    }
}