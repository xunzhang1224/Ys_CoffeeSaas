using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageCollectionCommands;
using YS.CoffeeMachine.Domain.IPlatformRepositories.BeveragesRepositorys;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.BeveragesCommandHandlers.BeverageCollectionCommandHandlers
{
    /// <summary>
    /// 编辑饮品集合
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="context"></param>
    public class UpdateBeverageCollectionCommandHandler(IPBeverageCollectionRepository repository, CoffeeMachinePlatformDbContext context) : ICommandHandler<UpdateBeverageCollectionCommand, bool>
    {
        /// <summary>
        /// 编辑饮品集合
        /// </summary>
        public async Task<bool> Handle(UpdateBeverageCollectionCommand request, CancellationToken cancellationToken)
        {
            var info = await repository.GetAsync(request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            info.Update(request.name);
            var res = await repository.UpdateAsync(info);
            return res != null;
        }
    }
}
