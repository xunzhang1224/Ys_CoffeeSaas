using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.AdvertisementsCommands.AdvertisementsInfoCommands
{
    public record UpdateAdvertisementInfoCommand(long id, long deviceInfoId, int powerOnAdsVolume, int powerOnAdsPlayTime, string powerOnAdsImagesJson,
        int standbyAdsVolume, int standbyAdsPlayTime, int standbyAdsAwaitTime, int standbyAdsLoopTime, bool standbyAdsLoopType, string StandbyAdsImagesJson,
        int productionAdsVolume, int productionAdsPlayTime, string productionAdsImagesJson) : ICommand<bool>;
}
