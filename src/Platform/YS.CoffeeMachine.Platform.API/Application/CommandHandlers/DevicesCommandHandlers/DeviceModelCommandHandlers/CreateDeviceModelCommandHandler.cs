using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceModelCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.IPlatformRepositories.DevicesIRepositories;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.DevicesCommandHandlers.DeviceModelCommandHandlers
{
    /// <summary>
    /// 创建设备型号
    /// </summary>
    /// <param name="repository"></param>
    public class CreateDeviceModelCommandHandler(IPDeviceModelRepository repository, CoffeeMachinePlatformDbContext context) : ICommandHandler<CreateDeviceModelCommand, bool>
    {
        /// <summary>
        /// 创建设备型号
        /// </summary>
        public async Task<bool> Handle(CreateDeviceModelCommand request, CancellationToken cancellationToken)
        {
            // 验证设备型号名是否存在
            var verify = await context.DeviceModel.AnyAsync(w => w.Name == request.name);
            if (verify)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0012)]);

            var info = new DeviceModel(request.key, request.name, request.maxCassetteCount, request.remark, request.type);
            var res = await repository.AddAsync(info);
            return res != null;
        }
    }
}