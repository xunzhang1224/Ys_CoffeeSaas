using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceInfoCommands
{
    public record UpdateDeviceInfoCommand(long id, string name, string equipmentNumber, /*long deviceModelId,*/ string versionNumber, string skinPluginVersion, string languagePack, DateTime? updateTime, string ssid, string mac, string iccid, string usedTrafficThisMonth, string remainingTrafficThisMonth, string latitude, string longitude, long? countryId, List<long> countryRegionIds, string countryRegionText, string detailedAddress, UsageScenarioEnum? usageScenario, string posMachineNumber, List<long>? groupIds, DeviceStatusEnum? deviceStatus, DateTime? latestOnlineTime, DateTime? latestOfflineTime) : ICommand<bool>;
    public record SetOnLineCommand(string mid, DeviceStatusEnum OnLineStatus, string time) : ICommand;

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="dics"></param>
    /// <param name="id"></param>
    public record UpdateDeviceInfoByDicCommand(Dictionary<string, object> dics, long id) : ICommand<bool>;

    /// <summary>
    /// 修改名称
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    public record UpdateDeviceNameCommand(long id, string name) : ICommand<bool>;

    public record UpdateDevicePointCommand(long id, string province, string city, string district, string street, string detailedAddress, decimal? lat, decimal? lng) : ICommand<bool>;
}
