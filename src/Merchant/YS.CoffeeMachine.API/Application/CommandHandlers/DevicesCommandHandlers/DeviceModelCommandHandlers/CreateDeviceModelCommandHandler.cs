using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceModelCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.IRepositories.DevicesIRepositories;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.DevicesCommandHandlers.DeviceModelCommandHandlers
{
    /// <summary>
    /// 创建设备型号
    /// </summary>
    /// <param name="repository"></param>
    public class CreateDeviceModelCommandHandler(IDeviceModelRepository repository) : ICommandHandler<CreateDeviceModelCommand, bool>
    {
        /// <summary>
        /// 创建设备型号
        /// </summary>
        public async Task<bool> Handle(CreateDeviceModelCommand request, CancellationToken cancellationToken)
        {
            var info = new DeviceModel(request.key, request.name, request.maxCassetteCount, request.remark, request.type);
            var res = await repository.AddAsync(info);
            return res != null;
        }
    }
}
