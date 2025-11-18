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
    /// 删除饮品合集
    /// </summary>
    /// <param name="context"></param>
    public class DeleteP_BeverageCollectionCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<DeleteP_BeverageCollectionCommand, bool>
    {
        /// <summary>
        /// 删除饮品合集
        /// </summary>
        public async Task<bool> Handle(DeleteP_BeverageCollectionCommand request, CancellationToken cancellationToken)
        {
            if (request.ids.Count <= 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0013)]);
            var lists = await context.P_BeverageCollection.Where(w => request.ids.Contains(w.Id)).ToListAsync();
            lists.ForEach(e => e.IsDelete = true);
            return true;
        }
    }
}
