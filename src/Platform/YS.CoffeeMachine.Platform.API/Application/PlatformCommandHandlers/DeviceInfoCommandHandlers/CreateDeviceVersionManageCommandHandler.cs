using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.PlatformCommands.DeviceInfoCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Infrastructure;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.DeviceInfoCommandHandlers
{
    /// <summary>
    /// 创建管理
    /// </summary>
    /// <param name="_context"></param>
    public class CreateDeviceVersionManageCommandHandler(CoffeeMachinePlatformDbContext _context) : ICommandHandler<CreateDeviceVersionManageCommand>
    {
        /// <summary>
        /// 创建管理
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Handle(CreateDeviceVersionManageCommand request, CancellationToken cancellationToken)
        {
            var info = new DeviceVersionManage(request.name, request.deviceType, request.versionNumber, request.deviceModelId, request.programType, request.versionType, request.url, request.remark, request.programTypeName);
            await _context.AddAsync(info);
        }
    }

    /// <summary>
    /// 更新管理状态
    /// </summary>
    public class UpdateDeviceVersionManageStateCommandHandler(CoffeeMachinePlatformDbContext _context) : ICommandHandler<UpdateDeviceVersionManageStateCommand>
    {
        /// <summary>
        /// 更新管理状态
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Handle(UpdateDeviceVersionManageStateCommand request, CancellationToken cancellationToken)
        {
            var info = await _context.DeviceVersionManage.AsQueryable().Where(a => a.Id == request.id).FirstOrDefaultAsync();
            info.UpdateState(request.enabled);
        }
    }
}
