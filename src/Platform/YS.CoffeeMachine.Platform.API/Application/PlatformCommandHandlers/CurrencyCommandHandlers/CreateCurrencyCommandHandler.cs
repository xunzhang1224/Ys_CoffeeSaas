using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.PlatformCommands.CurrencyCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.CurrencyCommandHandlers
{
    /// <summary>
    /// 创建币种
    /// </summary>
    /// <param name="context"></param>
    public class CreateCurrencyCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<CreateCurrencyCommand, bool>
    {
        /// <summary>
        /// 创建币种
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(CreateCurrencyCommand request, CancellationToken cancellationToken)
        {
            var exist = await context.Currency.AsQueryable().AnyAsync(a => a.Code == request.code || a.Name == request.name);
            if (exist)
                throw ExceptionHelper.AppFriendly("货币代码或名称重复");

            var info = new Currency(request.code, request.name, request.currencySymbol, request.currencyShowFormat, request.accuracy, request.roundingType, request.enabled);
            await context.AddAsync(info);
            return true;
        }
    }
}
