using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceModelCommands;
using YS.CoffeeMachine.Domain.IPlatformRepositories.DevicesIRepositories;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.DevicesCommandHandlers.DeviceModelCommandHandlers
{
    /// <summary>
    /// 删除设备型号
    /// </summary>
    /// <param name="repository"></param>
    public class DeleteDeviceModelCommandHandler(IPDeviceModelRepository repository) : ICommandHandler<DeleteDeviceModelCommand, bool>
    {
        /// <summary>
        /// 删除设备型号
        /// </summary>
        public async Task<bool> Handle(DeleteDeviceModelCommand request, CancellationToken cancellationToken)
        {
            if (request.id <= 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0013)]);
            var res = await repository.FakeDeleteByIdAsync(request.id);
            return res > 0;
        }
    }
}
