using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageCollectionCommands;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.BeveragesCommandHandlers.BeverageCollectionCommandHandlers
{
    /// <summary>
    /// 更新饮品合集
    /// </summary>
    /// <param name="context"></param>
    public class UpdateP_BeverageCollectionNameCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<UpdateP_BeverageCollectionNameCommand, bool>
    {
        /// <summary>
        /// 更新饮品合集
        /// </summary>
        public async Task<bool> Handle(UpdateP_BeverageCollectionNameCommand request, CancellationToken cancellationToken)
        {
            var info = await context.P_BeverageCollection.AsQueryable()
                .Where(a => a.Id == request.id)
                .FirstOrDefaultAsync();
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

            info.Update(request.name);
            return true;
        }
    }
}
