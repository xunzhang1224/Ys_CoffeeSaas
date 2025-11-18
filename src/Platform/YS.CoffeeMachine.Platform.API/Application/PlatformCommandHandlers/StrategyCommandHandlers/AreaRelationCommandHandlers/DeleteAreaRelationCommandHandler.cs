using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.PlatformCommands.StrategyCommands.AreaRelationCommands;
using YS.CoffeeMachine.Infrastructure;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.StrategyCommandHandlers.AreaRelationCommandHandlers
{
    /// <summary>
    /// 删除地区关联
    /// </summary>
    /// <param name="context"></param>
    public class DeleteAreaRelationCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<DeleteAreaRelationCommand, bool>
    {
        /// <summary>
        /// 删除地区关联
        /// </summary>
        public async Task<bool> Handle(DeleteAreaRelationCommand request, CancellationToken cancellationToken)
        {
            var enterpriseInfo = await context.EnterpriseInfo.AsQueryable()
                .Where(a => a.AreaRelationId == request.id).FirstOrDefaultAsync();
            if (enterpriseInfo != null)
                throw ExceptionHelper.AppFriendly("存在关联关系，不能删除");

            await context.AreaRelation.AsQueryable().Where(a => a.Id == request.id).ExecuteDeleteAsync();
            return true;
        }
    }
}
