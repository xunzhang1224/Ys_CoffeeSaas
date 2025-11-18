using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceInfoCommands
{
    public record CreateDeviceInfoCommand(string mid, string name, string equipmentNumber, long deviceModelId, string versionNumber
       , string skinPluginVersion, string languagePack, DateTime? updateTime, string ssid, string mac, string iccid, string usedTrafficThisMonth
       , string remainingTrafficThisMonth, string latitude, string longitude, long? countryId, List<long> countryRegionIds, string countryRegionText
       , string detailedAddress, UsageScenarioEnum usageScenario, string posMachineNumber, List<long> groupIds) : ICommand<bool>;
}