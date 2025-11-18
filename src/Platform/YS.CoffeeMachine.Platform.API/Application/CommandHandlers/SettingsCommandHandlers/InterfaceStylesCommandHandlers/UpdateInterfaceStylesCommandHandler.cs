using YS.CoffeeMachine.Application.Commands.SettingsCommands.InterfaceStylesCommands;
using YS.CoffeeMachine.Domain.IPlatformRepositories.SettingsIRepositories;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.SettingsCommandHandlers.InterfaceStylesCommandHandlers
{
    /// <summary>
    /// 编辑界面风格
    /// </summary>
    /// <param name="repository"></param>
    public class UpdateInterfaceStylesCommandHandler(IPInterfaceStylesRepository repository) : ICommandHandler<UpdateInterfaceStylesCommand, bool>
    {
        /// <summary>
        /// 编辑界面风格
        /// </summary>
        public async Task<bool> Handle(UpdateInterfaceStylesCommand request, CancellationToken cancellationToken)
        {
            var info = await repository.GetAsync(request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            info.Update(request.name, request.code, request.preview);
            var res = repository.UpdateAsync(info);
            return res != null;
        }
    }
}
