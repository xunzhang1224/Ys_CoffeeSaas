using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageCollectionCommands;
using YS.CoffeeMachine.Domain.IRepositories.BeveragesRepositorys;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.BeveragesCommandHandlers.BeverageCollectionCommandHandlers
{
    /// <summary>
    /// 编辑饮品集合
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="context"></param>
    public class UpdateBeverageCollectionCommandHandler(IBeverageCollectionRepository repository, CoffeeMachineDbContext context) : ICommandHandler<UpdateBeverageCollectionCommand, bool>
    {
        /// <summary>
        /// 编辑饮品集合
        /// </summary>
        public async Task<bool> Handle(UpdateBeverageCollectionCommand request, CancellationToken cancellationToken)
        {

            // 检查合集名称是否已存在
            var allbeverageCollectionCount = await context.BeverageCollection.Where(w => w.Name == request.name && w.Id != request.id).CountAsync();
            if (allbeverageCollectionCount > 0)
                throw ExceptionHelper.AppFriendly(string.Format(L.Text[nameof(ErrorCodeEnum.D0050)], request.name));

            var info = await repository.GetAsync(request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            info.Update(request.name);
            var res = await repository.UpdateAsync(info);
            return res != null;
        }
    }
}
