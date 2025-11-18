using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoCommands;
using YS.CoffeeMachine.Infrastructure;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.BeveragesCommandHandlers.BeverageInfoCommandHandlers
{
    /// <summary>
    /// 批量修改设备下饮品价格
    /// </summary>
    public class UpdateBeveragePriceCommandHandler(CoffeeMachineDbContext context) : ICommandHandler<UpdateBeveragePriceCommand, bool>
    {
        /// <summary>
        /// 批量修改设备下饮品价格
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(UpdateBeveragePriceCommand request, CancellationToken cancellationToken)
        {

            var infos = await context.BeverageInfo.AsQueryable()
                .Where(a => a.DeviceId == request.deviceId && !a.IsDefault).ToListAsync();

            foreach (var item in infos)
            {
                var info = request.beverageInfoPriceList.FirstOrDefault(a => a.Id == item.Id);
                if (info != null)
                {
                    item.UpdatePriceInfo(info.Price, info.DiscountedPrice);
                }
            }

            return await context.SaveChangesAsync() > 0;
        }
    }
}
