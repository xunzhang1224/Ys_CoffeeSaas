using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.PlatformCommands.CurrencyCommands;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.CurrencyCommandHandlers
{
    /// <summary>
    /// 更新货币
    /// </summary>
    /// <param name="context"></param>
    public class UpdateCurrencyCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<UpdateCurrencyCommand, bool>
    {
        /// <summary>
        /// 更新货币
        /// </summary>
        public async Task<bool> Handle(UpdateCurrencyCommand request, CancellationToken cancellationToken)
        {
            var exists = await context.Currency.AsQueryable().AnyAsync(w => w.Id != request.id && w.Name == request.name);
            if (exists)
                throw ExceptionHelper.AppFriendly("货币名称重复");

            var info = await context.Currency.AsQueryable()
                .Where(a => a.Id == request.id)
                .FirstOrDefaultAsync();
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            info.UpdateCurrency(request.name, request.currencySymbol, request.currencyShowFormat, request.enabled);
            return true;
            return await context.SaveChangesAsync() > 0;
        }
    }
}
