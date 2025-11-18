using YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.EnterpriseTypesCommands;
using YS.CoffeeMachine.Domain.IPlatformRepositories.ApplicationUsersIRepositories;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.ApplicationCommandHandlers.EnterpriseTypesCommandHandlers
{
    /// <summary>
    /// 删除企业类型命令处理程序
    /// </summary>
    /// <param name="repository"></param>
    public class DeleteEnterpriseTypesCommandHandler(IPEnterpriseTypesRepository repository) : ICommandHandler<DeleteEnterpriseTypesCommand, bool>
    {
        /// <summary>
        /// 删除企业类型命令处理程序
        /// </summary>
        public async Task<bool> Handle(DeleteEnterpriseTypesCommand request, CancellationToken cancellationToken)
        {
            throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0080)]);
            //if (request.id <= 0)
            //    throw ExceptionHelper.AppFriendly("Id不能为空{0}", request.id);
            //var res = await repository.FakeDeleteByIdAsync(request.id);
            //return res > 0;
        }
    }
}
