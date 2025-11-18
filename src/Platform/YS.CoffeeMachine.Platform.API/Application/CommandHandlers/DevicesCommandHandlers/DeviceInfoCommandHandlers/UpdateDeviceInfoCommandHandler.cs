using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceInfoCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.IRepositories.DevicesIRepositories;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.DevicesCommandHandlers.DeviceInfoCommandHandlers
{
    /// <summary>
    /// 编辑设备
    /// </summary>
    /// <param name="repository"></param>
    public class UpdateDeviceInfoCommandHandler(IDeviceInfoRepository repository) : ICommandHandler<UpdateDeviceInfoCommand, bool>
    {
        /// <summary>
        /// 编辑设备
        /// </summary>
        public async Task<bool> Handle(UpdateDeviceInfoCommand request, CancellationToken cancellationToken)
        {
            var info = await repository.GetByIdAsync(request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0008)]);
            var regionText = info.CountryRegionText;
            info.Update(request.name, request.devicePositionVo, request.usageScenario, request.pOSMachineNumber);

            //设备分组
            if (request.groupIds != null)
                info.BindGroups(request.groupIds);
            var res = repository.UpdateAsync(info);
            return res != null;
        }
    }
    /// <summary>
    /// 设置设备上下线
    /// </summary>
    /// <param name="repository"></param>
    public class SetOnLineCommandHandler(IDeviceInfoRepository repository) : ICommandHandler<SetOnLineCommand>
    {
        /// <summary>
        /// 设置设备上下线
        /// </summary>
        public async Task Handle(SetOnLineCommand request, CancellationToken cancellationToken)
        {
            //var info = await repository.GetByMidAsync(request.mid);
            //if (info == null)
            //    throw ExceptionHelper.AppFriendly(string.Format(L.Text[nameof(ErrorCodeEnum.D0005)], request.mid));
            //if (request.OnLineStatus)
            //    info.DeviceMetrics.OnLine(request.signal);
            //else
            //    info.DeviceMetrics.Offline();
        }
    }
}