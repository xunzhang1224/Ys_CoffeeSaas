using YS.CoffeeMachine.Application.Commands.SettingsCommands.InterfaceStylesCommands;
using YS.CoffeeMachine.Domain.IRepositories.SettingsIRepositories;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.SettingsCommandHandlers.InterfaceStylesCommandHandlers
{
    /// <summary>
    /// 删除风格
    /// </summary>
    /// <param name="repository"></param>
    public class DeleteInterfaceStylesCommandHandler(IInterfaceStylesRepository repository) : ICommandHandler<DeleteInterfaceStylesCommand, bool>
    {
        /// <summary>
        /// 删除风格
        /// </summary>
        public async Task<bool> Handle(DeleteInterfaceStylesCommand request, CancellationToken cancellationToken)
        {
            if (request.id <= 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0013)]);
            var res = await repository.FakeDeleteByIdAsync(request.id);
            return res > 0;
        }
    }
}
