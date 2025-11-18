namespace YS.CoffeeMachine.Domain.AggregatesModel.Devices
{
    using System.Data;
    using System.Globalization;
    using System.Reflection;
    using YS.CoffeeMachine.Domain.AggregatesModel.Advertisements;
    using YS.CoffeeMachine.Domain.AggregatesModel.Basics;
    using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
    using YS.CoffeeMachine.Domain.AggregatesModel.Card;
    using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
    using YS.CoffeeMachine.Domain.Events.BeveragesDomainEvents;
    using YS.CoffeeMachine.Domain.Events.DeviceDomainEvents;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 设备信息
    /// </summary>
    public class DeviceInfo : EnterpriseBaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 平台端设备id
        /// </summary>
        public long DeviceBaseId { get; private set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 激活时间
        /// </summary>
        public DateTime? ActiveTime { get; private set; }

        /// <summary>
        /// 设备激活状态
        /// </summary>
        public DeviceActiveEnum? DeviceActiveState { get; private set; }

        #region 规格参数

        /// <summary>
        /// 设备sn编号
        /// </summary>
        public string EquipmentNumber { get; private set; }

        /// <summary>
        /// 设备通讯编码
        /// </summary>
        public string? Mid { get; private set; }

        /// <summary>
        /// 设备型号Id
        /// </summary>
        public long? DeviceModelId { get; private set; }

        /// <summary>
        /// 设备型号
        /// </summary>
        public DeviceModel DeviceModel { get; private set; }

        /// <summary>
        /// 是否出厂
        /// </summary>
        public IsLeaveFactoryEnum? IsLeaveFactory { get; private set; }
        #endregion
        #region 软件参数

        /// <summary>
        /// 主程序版本号
        /// </summary>
        public string VersionNumber { get; private set; }

        /// <summary>
        /// 皮肤插件版本
        /// </summary>
        public string SkinPluginVersion { get; private set; }

        /// <summary>
        /// 语言包
        /// </summary>
        public string LanguagePack { get; private set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? UpdateTime { get; private set; }
        #endregion
        #region 网络参数

        /// <summary>
        /// WIFI SSID
        /// </summary>
        public string SSID { get; private set; }

        /// <summary>
        /// MAC地址
        /// </summary>
        public string MAC { get; private set; }

        /// <summary>
        /// 物联网卡信息
        /// </summary>
        public string ICCID { get; private set; }

        /// <summary>
        /// 本月已使用流量
        /// </summary>
        public string UsedTrafficThisMonth { get; private set; }

        /// <summary>
        /// 本月剩余流量
        /// </summary>
        public string RemainingTrafficThisMonth { get; private set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string Longitude { get; private set; }
        /// <summary>
        /// 维度
        /// </summary>
        public string Latitude { get; private set; }
        #endregion

        /// <summary>
        /// 最近上线时间
        /// </summary>
        public DateTime? LatestOnlineTime { get; private set; }
        /// <summary>
        /// 最近下线时间
        /// </summary>
        public DateTime? LatestOfflineTime { get; private set; }
        /// <summary>
        /// 国家Id
        /// </summary>
        public long? CountryId { get; private set; }
        /// <summary>
        /// 地区Ids
        /// </summary>
        public string CountryRegionIds { get; private set; }
        /// <summary>
        /// 国家地区完整字符串
        /// </summary>
        public string CountryRegionText { get; private set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string DetailedAddress { get; private set; }
        /// <summary>
        /// 设备状态
        /// </summary>
        public DeviceStatusEnum DeviceStatus { get; private set; }
        /// <summary>
        /// 使用场景
        /// </summary>
        public UsageScenarioEnum? UsageScenario { get; private set; }
        /// <summary>
        /// POS机编号
        /// </summary>
        public string POSMachineNumber { get; private set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; private set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; private set; }

        /// <summary>
        /// 区县
        /// </summary>
        public string District { get; private set; }

        /// <summary>
        /// 街道
        /// </summary>
        public string Street { get; private set; }

        /// <summary>
        /// 经度
        /// </summary>
        public decimal? Lat { get; private set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public decimal? Lng { get; private set; }

        /// <summary>
        /// 设备分组
        /// </summary>
        public List<GroupDevices> GroupDevices { get; private set; }
        /// <summary>
        /// 设备分配
        /// </summary>
        public List<EnterpriseDevices> EnterpriseDevices { get; private set; }
        /// <summary>
        /// 设备服务商
        /// </summary>
        public List<DeviceServiceProviders> DeviceServiceProviders { get; private set; }
        /// <summary>
        /// 设置参数
        /// </summary>
        public SettingInfo SettingInfo { get; private set; }
        /// <summary>
        /// 广告配置
        /// </summary>
        public AdvertisementInfo AdvertisementInfo { get; private set; }
        /// <summary>
        /// 预警配置
        /// </summary>
        public EarlyWarningConfig EarlyWarningConfig { get; private set; }
        /// <summary>
        /// 关联用户
        /// </summary>
        public List<DeviceUserAssociation> DeviceUserAssociations { get; private set; }

        /// <summary>
        /// 饮品集合
        /// </summary>
        public List<BeverageInfo> BeverageInfos { get; private set; }

        private readonly List<CardDeviceAssignment> _assignments = new List<CardDeviceAssignment>();

        /// <summary>
        /// 绑定卡集合
        /// </summary>
        public IReadOnlyCollection<CardDeviceAssignment> Assignments => _assignments.AsReadOnly();

        /// <summary>
        /// 设备
        /// </summary>
        protected DeviceInfo()
        {
        }

        /// <summary>
        /// 运营商绑定设备
        /// </summary>
        /// <param name="enterpriseinfoId"></param>
        /// <param name="deviceBaseId"></param>
        public DeviceInfo(long enterpriseinfoId, long deviceBaseId, long deviceModelId, string? deviceName = null)
        {
            EnterpriseinfoId = enterpriseinfoId;
            DeviceBaseId = deviceBaseId;
            DeviceModelId = deviceModelId;
            if (deviceName != null)
            {
                Name = deviceName;
            }
        }

        /// <summary>
        /// 运营商绑定设备
        /// </summary>
        /// <param name="enterpriseinfoId"></param>
        /// <param name="deviceBaseId"></param>
        /// <param name="deviceName">设备名称</param>
        /// <param name="equipmentNumber">生产编码（唯一标识）</param>
        public DeviceInfo(long enterpriseinfoId, long deviceBaseId, string deviceName, string equipmentNumber)
        {
            EnterpriseinfoId = enterpriseinfoId;
            DeviceBaseId = deviceBaseId;
            Name = deviceName;
            EquipmentNumber = equipmentNumber;
        }

        /// <summary>
        /// 创建设备
        /// </summary>
        /// </summary>
        /// <param name="enterpriseinfoId"></param>
        /// <param name="name"></param>
        /// <param name="equipmentNumber"></param>
        /// <param name="deviceModelId"></param>
        /// <param name="versionNumber"></param>
        /// <param name="skinPluginVersion"></param>
        /// <param name="languagePack"></param>
        /// <param name="updateTime"></param>
        /// <param name="ssid"></param>
        /// <param name="mac"></param>
        /// <param name="iccid"></param>
        /// <param name="usedTrafficThisMonth"></param>
        /// <param name="remainingTrafficThisMonth"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="countryId"></param>
        /// <param name="countryRegionIds"></param>
        /// <param name="countryRegionText"></param>
        /// <param name="detailedAddress"></param>
        /// <param name="usageScenario"></param>
        /// <param name="posMachineNumber"></param>
        public DeviceInfo(long enterpriseinfoId, string name, string equipmentNumber, long deviceModelId, string versionNumber, string skinPluginVersion, string languagePack, DateTime? updateTime, string ssid, string mac, string iccid, string usedTrafficThisMonth, string remainingTrafficThisMonth, string latitude, string longitude, long? countryId, string countryRegionIds, string countryRegionText, string detailedAddress, UsageScenarioEnum? usageScenario, string posMachineNumber)
        {
            EnterpriseinfoId = enterpriseinfoId;
            Name = name;
            EquipmentNumber = equipmentNumber;
            DeviceModelId = deviceModelId;
            VersionNumber = versionNumber;
            SkinPluginVersion = skinPluginVersion;
            LanguagePack = languagePack;
            UpdateTime = updateTime;
            SSID = ssid;
            MAC = mac;
            ICCID = iccid;
            UsedTrafficThisMonth = usedTrafficThisMonth;
            RemainingTrafficThisMonth = remainingTrafficThisMonth;
            Latitude = latitude;
            Longitude = longitude;
            CountryId = countryId;
            CountryRegionIds = countryRegionIds;
            CountryRegionText = countryRegionText;
            DetailedAddress = detailedAddress;
            UsageScenario = usageScenario;
            DeviceStatus = DeviceStatusEnum.Offline;//默认离线，等待安卓推送新的状态再更新
            POSMachineNumber = posMachineNumber;
            Mid = equipmentNumber;
            //添加设备时，自动创建预警配置信息
            //AddDomainEvent(new CreateEarlyWarningConfigDomainEvent(Id));
        }

        /// <summary>
        /// 创建设备
        /// </summary>
        /// <param name="enterpriseinfoId"></param>
        /// <param name="equipmentNumber"></param>
        /// <param name="deviceBaseId"></param>
        public DeviceInfo(long enterpriseinfoId, string equipmentNumber, long deviceBaseId)
        {
            EnterpriseinfoId = enterpriseinfoId;
            EquipmentNumber = equipmentNumber;
            DeviceBaseId = deviceBaseId;
        }

        /// <summary>
        /// 更新设备
        /// </summary>
        /// <param name="name"></param>
        /// <param name="equipmentNumber"></param>
        /// <param name="versionNumber"></param>
        /// <param name="skinPluginVersion"></param>
        /// <param name="languagePack"></param>
        /// <param name="updateTime"></param>
        /// <param name="ssid"></param>
        /// <param name="mac"></param>
        /// <param name="iccid"></param>
        /// <param name="usedTrafficThisMonth"></param>
        /// <param name="remainingTrafficThisMonth"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="countryId"></param>
        /// <param name="countryRegionIds"></param>
        /// <param name="countryRegionText"></param>
        /// <param name="detailedAddress"></param>
        /// <param name="posMachineNumber"></param>
        public void Update(string name, string equipmentNumber, /*long deviceModelId,*/ string versionNumber, string skinPluginVersion, string languagePack, DateTime? updateTime, string ssid, string mac, string iccid, string usedTrafficThisMonth, string remainingTrafficThisMonth, string latitude, string longitude, long? countryId, string countryRegionIds, string countryRegionText, string detailedAddress, string posMachineNumber)
        {
            Name = name;
            EquipmentNumber = equipmentNumber;
            //DeviceModelId = deviceModelId;
            VersionNumber = versionNumber;
            SkinPluginVersion = skinPluginVersion;
            LanguagePack = languagePack;
            UpdateTime = updateTime;
            SSID = ssid;
            MAC = mac;
            ICCID = iccid;
            UsedTrafficThisMonth = usedTrafficThisMonth;
            RemainingTrafficThisMonth = remainingTrafficThisMonth;
            Latitude = latitude;
            Longitude = longitude;
            CountryId = countryId;
            CountryRegionIds = countryRegionIds;
            CountryRegionText = countryRegionText;
            DetailedAddress = detailedAddress;
            POSMachineNumber = posMachineNumber;
        }

        /// <summary>
        /// 编辑或新增设备
        /// </summary>
        /// <param name="name"></param>
        /// <param name="devicePositionVo"></param>
        /// <param name="usageScenario"></param>
        /// <param name="groupDevices"></param>
        /// <param name="pOSMachineNumber"></param>
        public void Update(string name, DevicePositionVoInfo devicePositionVo, UsageScenarioEnum usageScenario, string pOSMachineNumber)
        {
            Name = name;
            UsageScenario = usageScenario;
            POSMachineNumber = pOSMachineNumber;
            CountryId = devicePositionVo.CountryId;
            CountryRegionIds = devicePositionVo.CountryRegionIds;
            DetailedAddress = devicePositionVo.DetailedAddress;
            CountryRegionText = devicePositionVo.CountryRegionText;
        }

        /// <summary>
        /// 更新设备状态
        /// </summary>
        /// <param name="deviceStatus"></param>
        public void UpdateStatus(DeviceStatusEnum deviceStatus)
        {
            DeviceStatus = deviceStatus;
        }
        /// <summary>
        /// 更新使用场景
        /// </summary>
        /// <param name="usageScenario"></param>
        public void UpdateUsageScenario(UsageScenarioEnum usageScenario)
        {
            UsageScenario = usageScenario;
        }
        /// <summary>
        /// 更新设备最近上线时间
        /// </summary>
        /// <param name="latestOnlineTime"></param>
        public void UpdateLatestOnlineTime(DateTime latestOnlineTime)
        {
            LatestOnlineTime = latestOnlineTime;
        }
        /// <summary>
        /// 更新设备最近下线状态
        /// </summary>
        /// <param name="latestOfflineTime"></param>
        public void UpdateLatestOfflineTime(DateTime latestOfflineTime)
        {
            LatestOfflineTime = latestOfflineTime;
        }

        /// <summary>
        /// 清除基础信息
        /// </summary>
        public void ClearBaseInfo()
        {
            Name = "-";
            EnterpriseinfoId = -1;
            IsDelete = true;
            DeviceModelId = null;
            DeviceModel = null;
            Name = string.Empty;
            EquipmentNumber = string.Empty;
            Mid = string.Empty;
            SkinPluginVersion = string.Empty;
            LanguagePack = string.Empty;
            SSID = string.Empty;
            MAC = string.Empty;
            ICCID = string.Empty;
            UsedTrafficThisMonth = string.Empty;
            RemainingTrafficThisMonth = string.Empty;
            Latitude = string.Empty;
            Longitude = string.Empty;
            LatestOnlineTime = null;
            LatestOfflineTime = null;
        }

        /// <summary>
        /// 绑定分组
        /// </summary>
        /// <param name="groupIds"></param>
        public void BindGroups(List<long> groupIds)
        {
            if (GroupDevices != null)
                GroupDevices.Clear();
            GroupDevices = groupIds.Select(s => new GroupDevices(s, Id)).ToList();
        }

        /// <summary>
        /// 清除分组
        /// </summary>
        public void RemoveGroups()
        {
            GroupDevices.Clear();
        }

        /// <summary>
        /// 清除关联用户
        /// </summary>
        public void RemoveUsers()
        {
            DeviceUserAssociations.Clear();
        }

        /// <summary>
        /// 换绑企业
        /// </summary>
        public void ChangebindEnterprise(long enterpriseId)
        {
            EnterpriseinfoId = enterpriseId;
        }

        /// <summary>
        /// 解绑企业
        /// </summary>
        public void UnbindEnterprise()
        {
            DeviceBaseId = 0;
            EnterpriseinfoId = -1;
            IsDelete = true;
        }

        /// <summary>
        /// 绑定用户
        /// </summary>
        /// <param name="usersIds"></param>
        public void BindUsers(List<long> usersIds)
        {
            DeviceUserAssociations.Clear();
            DeviceUserAssociations = usersIds.Select(s => new DeviceUserAssociation(Id, s)).ToList();
        }
        /// <summary>
        /// 绑定服务商
        /// </summary>
        /// <param name="serviceProvidersIds"></param>
        public void BindServiceProviders(List<long> serviceProvidersIds)
        {
            DeviceServiceProviders.Clear();
            DeviceServiceProviders = serviceProvidersIds.Select(s => new DeviceServiceProviders(s, Id)).ToList();
        }

        /// <summary>
        /// 清除饮品信息
        /// </summary>
        public void ClearBeverageInfos()
        {
            BeverageInfos?.Clear();
        }

        /// <summary>
        /// 重置饮品价格
        /// </summary>
        public void ResetBeveragePrices()
        {
            if (BeverageInfos != null)
                BeverageInfos.ForEach(e => e.UpdatePriceInfo(0, 0));
        }

        /// <summary>
        /// 创建设备设置信息
        /// </summary>
        /// <param name="isShowEquipmentNumber"></param>
        /// <param name="interfaceStylesId"></param>
        /// <param name="washType"></param>
        /// <param name="regularWashTime"></param>
        /// <param name="washWarning"></param>
        /// <param name="afterSalesPhone"></param>
        /// <param name="expectedUpdateTime"></param>
        /// <param name="screenBrightness"></param>
        /// <param name="deviceSound"></param>
        /// <param name="administratorPwd"></param>
        /// <param name="replenishmentOfficerPwd"></param>
        /// <param name="startTime"></param>
        /// <param name="startWeek"></param>
        /// <param name="endTime"></param>
        /// <param name="endWeek"></param>
        /// <param name="languageName"></param>
        /// <param name="currencyCode"></param>
        /// <param name="materialBoxs"></param>
        public void CreateSettingInfo(bool isShowEquipmentNumber, long interfaceStylesId, WashEnum washType, string regularWashTime, int? washWarning,
            string afterSalesPhone, string expectedUpdateTime, int screenBrightness, int deviceSound, string administratorPwd, string replenishmentOfficerPwd, string startTime,
            int startWeek, string endTime, int endWeek, string languageName, string currencyCode, List<MaterialBox> materialBoxs)
        {
            if (SettingInfo == null)
            {
                SettingInfo = new SettingInfo(Id, isShowEquipmentNumber, interfaceStylesId, washType, regularWashTime, washWarning,
                    afterSalesPhone, expectedUpdateTime, screenBrightness, deviceSound, administratorPwd, replenishmentOfficerPwd, startTime,
                    startWeek, endTime, endWeek, languageName, currencyCode, materialBoxs);
                // TODO：需要配置默认货币
                SettingInfo.SetCurrencyInfo(currencyCode, string.Empty, string.Empty, 0, 2);
            }
        }

        /// <summary>
        /// 创建设备预警信息
        /// </summary>
        /// <param name="deviceInfoId"></param>
        /// <param name="wholeMachineCleaningSwitch"></param>
        /// <param name="nextWholeMachineCleaningTime"></param>
        /// <param name="brewingMachineCleaningSwitch"></param>
        /// <param name="nextBrewingMachineCleaningTime"></param>
        /// <param name="milkFrotherCleaningSwitch"></param>
        /// <param name="nextMilkFrotherCleaningTime"></param>
        /// <param name="coffeeWaterwayCleaningSwitch"></param>
        /// <param name="nextCoffeeWaterwayCleaningTime"></param>
        /// <param name="steamWaterwayCleaningSwitch"></param>
        /// <param name="nextSteamWaterwayCleaningTime"></param>
        /// <param name="offlineWarningSwitch"></param>
        /// <param name="offlineDays"></param>
        /// <param name="shortageWarningSwitch"></param>
        /// <param name="coffeeBeanRemaining"></param>
        /// <param name="waterRemaining"></param>
        public void CreateDefaultEarlyWarningConfig(long deviceInfoId, bool wholeMachineCleaningSwitch, DateTime nextWholeMachineCleaningTime, bool brewingMachineCleaningSwitch, DateTime nextBrewingMachineCleaningTime,
        bool milkFrotherCleaningSwitch, DateTime nextMilkFrotherCleaningTime, bool coffeeWaterwayCleaningSwitch, DateTime nextCoffeeWaterwayCleaningTime, bool steamWaterwayCleaningSwitch, DateTime nextSteamWaterwayCleaningTime,
        bool offlineWarningSwitch, int offlineDays, bool shortageWarningSwitch, double coffeeBeanRemaining, double waterRemaining)
        {
            EarlyWarningConfig = new EarlyWarningConfig(deviceInfoId, wholeMachineCleaningSwitch, nextWholeMachineCleaningTime, brewingMachineCleaningSwitch, nextBrewingMachineCleaningTime, milkFrotherCleaningSwitch, nextMilkFrotherCleaningTime, coffeeWaterwayCleaningSwitch, nextCoffeeWaterwayCleaningTime, steamWaterwayCleaningSwitch, nextSteamWaterwayCleaningTime, offlineWarningSwitch, offlineDays, shortageWarningSwitch, coffeeBeanRemaining, waterRemaining);
        }

        /// <summary>
        /// 修改设备预警信息
        /// </summary>
        /// <param name="deviceInfoId"></param>
        /// <param name="wholeMachineCleaningSwitch"></param>
        /// <param name="nextWholeMachineCleaningTime"></param>
        /// <param name="brewingMachineCleaningSwitch"></param>
        /// <param name="nextBrewingMachineCleaningTime"></param>
        /// <param name="milkFrotherCleaningSwitch"></param>
        /// <param name="nextMilkFrotherCleaningTime"></param>
        /// <param name="coffeeWaterwayCleaningSwitch"></param>
        /// <param name="nextCoffeeWaterwayCleaningTime"></param>
        /// <param name="steamWaterwayCleaningSwitch"></param>
        /// <param name="nextSteamWaterwayCleaningTime"></param>
        /// <param name="offlineWarningSwitch"></param>
        /// <param name="offlineDays"></param>
        /// <param name="shortageWarningSwitch"></param>
        /// <param name="coffeeBeanRemaining"></param>
        /// <param name="waterRemaining"></param>
        public void UpdateEarlyWarningConfig(long deviceInfoId, bool wholeMachineCleaningSwitch, DateTime nextWholeMachineCleaningTime, bool brewingMachineCleaningSwitch, DateTime nextBrewingMachineCleaningTime,
        bool milkFrotherCleaningSwitch, DateTime nextMilkFrotherCleaningTime, bool coffeeWaterwayCleaningSwitch, DateTime nextCoffeeWaterwayCleaningTime, bool steamWaterwayCleaningSwitch, DateTime nextSteamWaterwayCleaningTime,
            bool offlineWarningSwitch, int offlineDays, bool shortageWarningSwitch, double coffeeBeanRemaining, double waterRemaining)
        {
            EarlyWarningConfig.Update(deviceInfoId, wholeMachineCleaningSwitch, nextWholeMachineCleaningTime, brewingMachineCleaningSwitch, nextBrewingMachineCleaningTime, milkFrotherCleaningSwitch, nextMilkFrotherCleaningTime, coffeeWaterwayCleaningSwitch, nextCoffeeWaterwayCleaningTime, steamWaterwayCleaningSwitch, nextSteamWaterwayCleaningTime, offlineWarningSwitch, offlineDays, shortageWarningSwitch, coffeeBeanRemaining, waterRemaining);
        }

        /// <summary>
        /// 绑定mid
        /// </summary>
        /// <param name="mid"></param>
        public void BindMid(string mid)
        {
            Mid = mid;
        }

        /// <summary>
        /// 设置硬件信息
        /// </summary>
        /// <param name="deviceHardwares"></param>
        public void SetDeviceHardwares(List<DeviceHardwares> deviceHardwares)
        {
            foreach (DeviceHardwares item in deviceHardwares)
            {
                //var old = DeviceHardwares.FirstOrDefault(x => x.Code == item.Code);
                //if (old != null)
                //    old.UpdateStatus(item.Status, item.Message);
                //else
                //    DeviceHardwares.Add(item);
            }
        }

        /// <summary>
        /// 设置上线
        /// </summary>
        /// <param name="time"></param>
        public void SetOnLine(string time)
        {
            DeviceStatus = DeviceStatusEnum.Online;
            LatestOnlineTime = Convert.ToDateTime(time);
        }

        /// <summary>
        /// 离线
        /// </summary>
        public void SetOffLine(string time)
        {
            DeviceStatus = DeviceStatusEnum.Offline;
            LatestOfflineTime = Convert.ToDateTime(time);
        }

        // <summary>
        /// 更新设备型号名称
        /// </summary>
        public void UpdateModelName(string modleName)
        {
            DeviceModel.Update(modleName, DeviceModel.MaxCassetteCount, DeviceModel.Remark);
        }

        // <summary>
        /// 更新设备型号最大纸杯数
        /// </summary>
        public void UpdateModelMaxCassetteCount(int maxCassetteCount)
        {
            DeviceModel.Update(DeviceModel.Name, maxCassetteCount, DeviceModel.Remark);
        }

        // <summary>
        /// 添加设备属性
        /// </summary>
        public void UpdateAttributeReport(string versionNumber, string skinPluginVersion, string languagePack, string ssid, string mac, string iccid, string usedTrafficThisMonth, string remainingTrafficThisMonth, string latitude, string longitude)
        {
            VersionNumber = versionNumber;
            SkinPluginVersion = skinPluginVersion;
            LanguagePack = languagePack;
            SSID = ssid;
            MAC = mac;
            ICCID = iccid;
            UsedTrafficThisMonth = usedTrafficThisMonth;
            RemainingTrafficThisMonth = remainingTrafficThisMonth;
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        /// 设置设备饮品
        /// </summary>
        /// <param name="beverages"></param>
        /// <param name="isSend">是否需要下发设备</param>
        public void SetBeverageInfos(List<BeverageInfo> beverages, bool isSend = true)
        {
            if (BeverageInfos != null && BeverageInfos.Count > 0)
                foreach (var item in BeverageInfos)
                {
                    item.AddCode(string.Empty);
                    item.IsDelete = true;
                }
            else
                BeverageInfos = new List<BeverageInfo>();
            BeverageInfos = beverages;
            AddDomainEvent(new CreateBeveragesDomainEvent(this, isSend));
        }

        /// <summary>
        /// 批量添加饮品
        /// </summary>
        /// <param name="beverageInfos"></param>
        public void AddBeverageInfo(List<BeverageInfo> beverageInfos)
        {
            if (BeverageInfos == null)
                BeverageInfos = new List<BeverageInfo>();
            BeverageInfos.AddRange(beverageInfos);
        }

        /// <summary>
        /// 批量设置饮品价格与币种
        /// </summary>
        /// <param name="beverages"></param>
        /// <param name="isSend">是否需要下发设备</param>
        public void SetRangeBeveragePrices(List<BeverageInfo> beverages, string currencyCode, bool isSend = true)
        {
            foreach (var item in BeverageInfos)
            {
                var newPrice = beverages.First(x => x.Code == item.Code);
                item.UpdatePriceInfo(newPrice.Price, newPrice.DiscountedPrice);
            }
            SettingInfo.SetCurrencyCode(currencyCode);
            AddDomainEvent(new SetRangeBeveragePricesDomainEvent(this, isSend));
        }
        /// <summary>
        /// 配置广告信息
        /// </summary>
        /// <param name="advertisement"></param>
        /// <param name="isSend">是否需要下发设备</param>
        public void SetAdvertisementInfo(AdvertisementInfo advertisement, bool isSend = true)
        {
            AdvertisementInfo = advertisement;
            AddDomainEvent(new SetAdvertisementDomainEvent(this, isSend));
        }

        /// <summary>
        /// 配置设置信息
        /// </summary>
        public void SetSettingInfo(Dictionary<string, string> diffs, bool isSend = true)
        {
            AddDomainEvent(new SetSettingInfoDomainEvent(this, diffs, isSend));
        }

        /// <summary>
        /// 重置设置信息
        /// </summary>
        public void ReSetSettingInfo()
        {
            SettingInfo.ReSetSettingInfo();
        }

        /// <summary>
        /// 激活设备
        /// </summary>
        public void ActiveDevice()
        {
            DeviceActiveState = DeviceActiveEnum.Active;
            ActiveTime = DateTime.UtcNow;
        }
        /// <summary>
        /// 根据字典更新设备信息
        /// </summary>
        /// <param name="updateDictionary">包含字段名和值的字典</param>
        public void UpdateFromDictionary(Dictionary<string, object> updateDictionary)
        {
            if (updateDictionary == null || updateDictionary.Count == 0)
                return;

            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var kvp in updateDictionary)
            {
                var property = properties.FirstOrDefault(p =>
                    p.Name.Equals(kvp.Key, StringComparison.OrdinalIgnoreCase) &&
                    p.CanWrite);

                if (property != null)
                {
                    try
                    {
                        SetPropertyValue(property, kvp.Value);
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException($"无法将值 '{kvp.Value}' 转换为属性 '{property.Name}' 的类型 '{property.PropertyType.Name}'", ex);
                    }
                }
            }
        }

        /// <summary>
        /// 更新设备名称
        /// </summary>
        /// <param name="name"></param>
        public void UpdateName(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 更新设备位置信息
        /// </summary>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <param name="district"></param>
        /// <param name="street"></param>
        /// <param name="detailedAddress"></param>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        public void UpdatePoint(string province, string city, string district, string street, string detailedAddress, decimal? lat, decimal? lng)
        {
            Province = province;
            City = city;
            District = district;
            Street = street;
            DetailedAddress = detailedAddress;
            Lat = lat;
            Lng = lng;
        }

        /// <summary>
        /// 设置属性值，处理各种类型转换
        /// </summary>
        private void SetPropertyValue(PropertyInfo property, object value)
        {
            if (value == null)
            {
                // 如果值为null，且属性是可空类型，则设置为null
                if (IsNullableType(property.PropertyType))
                {
                    property.SetValue(this, null);
                }
                return;
            }

            var targetType = property.PropertyType;
            var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            // 处理枚举类型
            if (underlyingType.IsEnum)
            {
                if (value is string stringValue)
                {
                    var enumValue = Enum.Parse(underlyingType, stringValue);
                    property.SetValue(this, enumValue);
                }
                else if (value is int intValue)
                {
                    var enumValue = Enum.ToObject(underlyingType, intValue);
                    property.SetValue(this, enumValue);
                }
                return;
            }

            // 处理字符串类型
            if (underlyingType == typeof(string))
            {
                property.SetValue(this, value.ToString());
                return;
            }

            // 处理布尔类型
            if (underlyingType == typeof(bool))
            {
                if (value is string boolString)
                {
                    var boolValue = boolString.ToLower() switch
                    {
                        "true" => true,
                        "false" => false,
                        "1" => true,
                        "0" => false,
                        _ => throw new FormatException($"无法将 '{boolString}' 转换为布尔值")
                    };
                    property.SetValue(this, boolValue);
                }
                else
                {
                    property.SetValue(this, Convert.ToBoolean(value));
                }
                return;
            }

            // 处理数值类型
            if (underlyingType == typeof(int) || underlyingType == typeof(long) ||
                underlyingType == typeof(double) || underlyingType == typeof(decimal) ||
                underlyingType == typeof(float) || underlyingType == typeof(short))
            {
                if (value is string numericString)
                {
                    var numericValue = Convert.ChangeType(numericString, underlyingType);
                    property.SetValue(this, numericValue);
                }
                else
                {
                    var numericValue = Convert.ChangeType(value, underlyingType);
                    property.SetValue(this, numericValue);
                }
                return;
            }

            // 处理DateTime类型
            if (underlyingType == typeof(DateTime))
            {
                if (value is string dateString)
                {
                    var dateValue = DateTime.Parse(dateString);
                    property.SetValue(this, dateValue);
                }
                else if (value is DateTime dateValue)
                {
                    property.SetValue(this, dateValue);
                }
                return;
            }

            // 默认情况：尝试直接转换
            try
            {
                var convertedValue = Convert.ChangeType(value, underlyingType);
                property.SetValue(this, convertedValue);
            }
            catch
            {
                throw new InvalidCastException($"无法将类型 {value.GetType().Name} 转换为 {underlyingType.Name}");
            }
        }

        /// <summary>
        /// 检查是否为可空类型
        /// </summary>
        private bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}