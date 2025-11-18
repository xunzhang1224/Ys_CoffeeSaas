using Microsoft.EntityFrameworkCore;
using Polly;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceInfoCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.DevicesCommandHandlers.DeviceInfoCommandHandlers
{
    /// <summary>
    /// 设备绑定
    /// </summary>
    /// <param name="_context"></param>
    public class BindDeviceCommandHandler(CoffeeMachineDbContext _context, UserHttpContext _user) : ICommandHandler<BindDeviceCommand>
    {
        /// <summary>
        /// 设备绑定
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(BindDeviceCommand request, CancellationToken cancellationToken)
        {
            var exixt = await _context.DeviceInfo.AsQueryable().IgnoreQueryFilters().Where(w => w.DeviceBaseId == request.deviceBaseInfoId && !w.IsDelete).FirstOrDefaultAsync();
            if (exixt != null)
            {
                if (_user.TenantId != exixt.EnterpriseinfoId)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0089)]);
                else
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0091)]);
            }

            var deviceBaseInfo = await _context.DeviceBaseInfo.AsQueryable().IgnoreQueryFilters().Where(w => w.Id == request.deviceBaseInfoId).FirstOrDefaultAsync();

            var info = new DeviceInfo(_user.TenantId, request.deviceBaseInfoId, deviceBaseInfo.DeviceModelId ?? 0, request.deviceName);
            await _context.AddAsync(info);
        }
    }
}
