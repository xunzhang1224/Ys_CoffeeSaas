using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.EarlyWarningConfigCommands;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.DevicesCommandHandlers.EarlyWarningConfigCommandHandlers
{
    /// <summary>
    /// 更新预警配置
    /// </summary>
    /// <param name="context"></param>
    public class UpdateEarlyWarningConfigCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<UpdateEarlyWarningConfigCommand, bool>
    {
        /// <summary>
        /// 更新预警配置
        /// </summary>
        public async Task<bool> Handle(UpdateEarlyWarningConfigCommand request, CancellationToken cancellationToken)
        {
            var info = await context.DeviceInfo.Include(i => i.EarlyWarningConfig).FirstOrDefaultAsync(w => w.Id == request.deviceInfoId);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            info.UpdateEarlyWarningConfig(request.deviceInfoId, request.wholeMachineCleaningSwitch, request.nextWholeMachineCleaningTime, request.brewingMachineCleaningSwitch, request.nextBrewingMachineCleaningTime, request.milkFrotherCleaningSwitch, request.nextMilkFrotherCleaningTime, request.coffeeWaterwayCleaningSwitch, request.nextCoffeeWaterwayCleaningTime, request.steamWaterwayCleaningSwitch, request.nextSteamWaterwayCleaningTime, request.offlineWarningSwitch, request.offlineDays, request.shortageWarningSwitch, request.coffeeBeanRemaining, request.waterRemaining);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
