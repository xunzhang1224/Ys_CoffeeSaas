using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.AdvertisementsCommands.AdvertisementsInfoCommands;
using YS.CoffeeMachine.Infrastructure;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.AdvertisementsCommandHandlers
{
    /// <summary>
    /// 编辑广告信息
    /// </summary>
    /// <param name="dbContext"></param>
    public class UpdateAdvertisementInfoCommandHandler(CoffeeMachinePlatformDbContext dbContext) : ICommandHandler<UpdateAdvertisementInfoCommand, bool>
    {
        /// <summary>
        /// 编辑广告信息
        /// </summary>
        public async Task<bool> Handle(UpdateAdvertisementInfoCommand request, CancellationToken cancellationToken)
        {
            //var info = await repository.GetAsync(request.id);
            //if (info == null)
            //    throw ExceptionHelper.AppFriendly("数据不存在");
            //info.Update(request.powerOnAdsVolume, request.powerOnAdsPlayTime, request.powerOnAdsImagesJson, request.standbyAdsVolume, request.standbyAdsPlayTime, request.standbyAdsAwaitTime, request.standbyAdsLoopType, request.StandbyAdsImagesJson, request.productionAdsVolume, request.productionAdsPlayTime, request.productionAdsImagesJson);
            //var res = repository.UpdateAsync(info);
            //return res != null;
            var device = await dbContext.DeviceInfo.Include(x => x.AdvertisementInfo).Where(y => y.AdvertisementInfo.Id == request.id).FirstAsync();
            device.AdvertisementInfo.Update(request.powerOnAdsVolume, request.powerOnAdsPlayTime, request.powerOnAdsImagesJson, request.standbyAdsVolume, request.standbyAdsPlayTime, request.standbyAdsAwaitTime, request.standbyAdsLoopTime, request.standbyAdsLoopType, request.StandbyAdsImagesJson, request.productionAdsVolume, request.productionAdsPlayTime, request.productionAdsImagesJson);
            device.SetAdvertisementInfo(device.AdvertisementInfo);
            return true;
        }
    }
}
