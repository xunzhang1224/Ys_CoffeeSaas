using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoCommands;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Exceptions.Extensions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.BeveragesCommandHandlers.BeverageInfoCommandHandlers
{
    /// <summary>
    /// 更新饮品价格
    /// </summary>
    /// <param name="context"></param>
    public class UpdateBevergePricesCommandHandler(CoffeeMachineDbContext context) : ICommandHandler<UpdateBevergePricesCommand, bool>
    {
        /// <summary>
        /// 更新饮品价格
        /// </summary>
        public async Task<bool> Handle(UpdateBevergePricesCommand request, CancellationToken cancellationToken)
        {
            var beverageInfos = await context.BeverageInfo.Where(w => request.PriceInfos.Select(s => s.Id).ToList().Contains(w.Id)).ToListAsync();
            foreach (var item in beverageInfos)
            {
                var priceInfo = request.PriceInfos.FirstOrDefault(f => f.Id == item.Id);
                if (priceInfo != null)
                {
                    item.UpdatePriceInfo(priceInfo.Price, priceInfo.DiscountedPrice);
                }
            }
            bool isSuccess = await context.SaveChangesAsync() > 0;
            if (!isSuccess)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0085)]).StatusCode(200).WithData(false);
            return isSuccess;
            return await context.SaveChangesAsync() > 0;
        }
    }
}
