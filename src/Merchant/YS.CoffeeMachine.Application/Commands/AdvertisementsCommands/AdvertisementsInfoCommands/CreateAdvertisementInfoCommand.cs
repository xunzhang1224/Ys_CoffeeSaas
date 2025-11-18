using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.AdvertisementsCommands.AdvertisementsInfoCommands
{
    public record CreateAdvertisementInfoCommand(long deviceInfoId, int powerOnAdsVolume, int powerOnAdsPlayTime, string powerOnAdsImagesJson,
        int standbyAdsVolume, int standbyAdsPlayTime, int standbyAdsAwaitTime, int StandbyAdsLoopTime, bool standbyAdsLoopType, string StandbyAdsImagesJson,
        int productionAdsVolume, int productionAdsPlayTime, string productionAdsImagesJson) : ICommand<bool>;
}
