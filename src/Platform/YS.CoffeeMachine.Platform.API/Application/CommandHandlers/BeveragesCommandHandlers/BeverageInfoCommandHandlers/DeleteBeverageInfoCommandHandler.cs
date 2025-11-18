using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoCommands;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.BeveragesCommandHandlers.BeverageInfoCommandHandlers
{
    /// <summary>
    /// 批量删除饮品
    /// </summary>
    /// <param name="context"></param>
    public class DeleteBeverageInfoCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<DeleteBeverageInfoCommand, bool>
    {
        /// <summary>
        /// 批量删除饮品
        /// </summary>
        public async Task<bool> Handle(DeleteBeverageInfoCommand request, CancellationToken cancellationToken)
        {
            if (request.ids.Count() == 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0013)]);
            var info = await context.P_BeverageInfo.Where(w => request.ids.Contains(w.Id)).ToListAsync();
            if (info == null || info.Count == 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            foreach (var item in info)
            {
                if (item.IsDefault)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0057)]);
                item.IsDelete = true;
                //item.AddCode(item.Code);
            }
            return true;
        }
    }
}
