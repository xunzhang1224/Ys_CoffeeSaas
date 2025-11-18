using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageCollectionCommands;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.BeveragesCommandHandlers.BeverageCollectionCommandHandlers
{
    /// <summary>
    /// 删除饮品合集
    /// </summary>
    /// <param name="context"></param>
    public class DeleteBeverageCollectionCommandHandler(CoffeeMachineDbContext context) : ICommandHandler<DeleteBeverageCollectionCommand, bool>
    {
        /// <summary>
        /// 删除饮品合集
        /// </summary>
        public async Task<bool> Handle(DeleteBeverageCollectionCommand request, CancellationToken cancellationToken)
        {
            if (request.ids.Count <= 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0013)]);
            var lists = await context.BeverageCollection.Where(w => request.ids.Contains(w.Id)).ToListAsync();

            var beverageCollectionVersionIds = new List<long>();
            foreach (var item in lists)
            {
                item.IsDelete = true;
                beverageCollectionVersionIds.Concat(item.BeverageIds.Split(",").Select(s => long.Parse(s)));
            }

            // 删除文件关联
            context.FileRelation.RemoveRange(context.FileRelation.Where(w => beverageCollectionVersionIds.Contains(w.TargetId)));

            //lists.ForEach(e => e.IsDelete = true);
            return true;
        }
    }
}
