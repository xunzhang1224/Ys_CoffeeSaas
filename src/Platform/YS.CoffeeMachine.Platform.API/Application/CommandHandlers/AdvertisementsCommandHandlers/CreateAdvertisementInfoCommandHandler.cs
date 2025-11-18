using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.AdvertisementsCommands.AdvertisementsInfoCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Advertisements;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.AdvertisementsCommandHandlers
{
    /// <summary>
    /// 添加广告信息
    /// </summary>
    /// <param name="context"></param>
    public class CreateAdvertisementInfoCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<CreateAdvertisementInfoCommand, bool>
    {
        /// <summary>
        /// 添加广告信息
        /// </summary>
        public async Task<bool> Handle(CreateAdvertisementInfoCommand request, CancellationToken cancellationToken)
        {
            var deviceInfo = await context.DeviceInfo.Include(i => i.AdvertisementInfo).FirstOrDefaultAsync(w => w.Id == request.deviceInfoId);
            if (deviceInfo == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0008)]);
            if (deviceInfo.SettingInfo != null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0009)]);
            var info = new AdvertisementInfo(request.deviceInfoId, request.powerOnAdsVolume, request.powerOnAdsPlayTime, request.powerOnAdsImagesJson, request.standbyAdsVolume, request.standbyAdsPlayTime, request.standbyAdsAwaitTime, request.StandbyAdsLoopTime, request.standbyAdsLoopType, request.StandbyAdsImagesJson, request.productionAdsVolume, request.productionAdsPlayTime, request.productionAdsImagesJson);
            //var res = repository.AddAsync(info);
            //return res != null;
            deviceInfo.SetAdvertisementInfo(info);
            return true;
        }
    }
}
