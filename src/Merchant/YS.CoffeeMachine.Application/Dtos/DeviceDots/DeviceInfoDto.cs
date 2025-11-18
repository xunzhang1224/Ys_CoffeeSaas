using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YS.CoffeeMachine.Application.Dtos.AdvertisementDtos;
using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YS.CoffeeMachine.Application.Dtos.SettringDtos;
using YS.CoffeeMachine.Application.Tools;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;

namespace YS.CoffeeMachine.Application.Dtos.DeviceDots
{
    /// <summary>
    /// 设备下拉选择
    /// </summary>
    public class DeviceSelectDto
    {
        /// <summary>
        /// 设备BaseIdId
        /// </summary>
        public long? Id { get; set; } = null;

        /// <summary>
        /// 设备Id
        /// </summary>
        public long DeviceId { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 设备永久编码
        /// </summary>
        public string Code { get; set; }
    }

    /// <summary>
    /// 设备信息
    /// </summary>
    public class DeviceInfoDto
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 设备基础信息Id
        /// </summary>
        public long DeviceBaseInfoId { get; set; }

        /// <summary>
        /// 设备基础信息Id
        /// </summary>
        public long DeviceBaseId { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceModelName { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Mid（设备唯一标识）
        /// </summary>
        public string? Mid { get; set; } = null;
        /// <summary>
        /// 车贴码/生产编号
        /// </summary>
        public string? MachineStickerCode { get; set; } = null;
        /// <summary>
        /// 箱体id
        /// </summary>
        public string BoxId { get; set; }

        /// <summary>
        /// 硬件能力
        /// </summary>
        public int? HardwareCapability { get; set; }
        /// <summary>
        /// 软件能力
        /// </summary>
        public int? SoftwareCapability { get; set; }
        /// <summary>
        /// 软件最后更新时间
        /// </summary>
        public DateTime? SoftwareUpdateLastTime { get; set; }
        /// <summary>
        /// 是否在线
        /// </summary>
        public bool? IsOnline { get; set; }
        /// <summary>
        /// 上线时间
        /// </summary>
        public DateTime? UpdateOnlineTime { get; set; }
        /// <summary>
        /// 下线时间
        /// </summary>
        public DateTime? UpdateOfflineTime { get; set; }

        #region 规格参数
        /// <summary>
        /// 设备编号
        /// </summary>
        public string EquipmentNumber { get; set; }
        /// <summary>
        /// 设备型号Id
        /// </summary>
        public long? DeviceModelId { get; set; }

        /// <summary>
        /// 设备型号
        /// </summary>
        public string DeviceModelText { get; set; }
        /// <summary>
        /// 是否出厂
        /// </summary>
        public IsLeaveFactoryEnum IsLeaveFactory { get; set; }
        #endregion
        #region 软件参数
        /// <summary>
        /// 主程序版本号
        /// </summary>
        public string VersionNumber { get; set; }
        /// <summary>
        /// 皮肤插件版本
        /// </summary>
        public string SkinPluginVersion { get; set; }
        /// <summary>
        /// 语言包
        /// </summary>
        public string LanguagePack { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        #endregion
        #region 网络参数
        /// <summary>
        /// WIFI SSID
        /// </summary>
        public string SSID { get; set; }
        /// <summary>
        /// MAC地址
        /// </summary>
        public string MAC { get; set; }
        /// <summary>
        /// 物联网卡信息
        /// </summary>
        public string ICCID { get; set; }
        /// <summary>
        /// 本月已使用流量
        /// </summary>
        public string UsedTrafficThisMonth { get; set; }
        /// <summary>
        /// 本月剩余流量
        /// </summary>
        public string RemainingTrafficThisMonth { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// 维度
        /// </summary>
        public string Latitude { get; set; }
        #endregion
        /// <summary>
        /// 设备分组
        /// </summary>
        public List<long> GroupIds { get; set; }

        /// <summary>
        /// 设备分组字符串
        /// </summary>
        public string GroupsText { get; set; }

        /// <summary>
        /// 最近上线时间
        /// </summary>
        public DateTime? LatestOnlineTime { get; set; }
        /// <summary>
        /// 最近下线时间
        /// </summary>
        public DateTime? LatestOfflineTime { get; set; }
        /// <summary>
        /// 国家Id
        /// </summary>
        public long? CountryId { get; set; }

        /// <summary>
        /// 国家字符串
        /// </summary>
        public string CountryText { get; set; }

        /// <summary>
        /// 国家地区Id
        /// </summary>
        public string CountryRegionIds { get; set; }
        /// <summary>
        /// 国家地区完整字符串
        /// </summary>
        public string CountryRegionText { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string DetailedAddress { get; set; }
        /// <summary>
        /// 设备在线状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 设备状态
        /// </summary>
        public DeviceStatusEnum DeviceStatus { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 区县
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// 街道
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public decimal? Lat { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public decimal? Lng { get; set; }

        /// <summary>
        /// 设备状态字符串
        /// </summary>
        public string DeviceStatusText { get { return EnumExtensions.GetDescriptionsByInt<DeviceStatusEnum>((int)DeviceStatus); } }
        /// <summary>
        /// 使用场景
        /// </summary>
        public UsageScenarioEnum? UsageScenario { get; set; }

        /// <summary>
        /// 使用场景名称
        /// </summary>
        public string UsageScenarioText { get; set; }

        /// <summary>
        /// POS机编号
        /// </summary>
        public string POSMachineNumber { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建时间字符串
        /// </summary>
        public string CreateTimeText { get { return CreateTime.ToString("G"); } }
        /// <summary>
        /// 用户Id集合
        /// </summary>
        public List<long> UserIds { get; set; } = [];

        /// <summary>
        /// 饮品集合
        /// </summary>
        public List<object> BeveragesList { get; set; }
        /// <summary>
        /// 设备分组
        /// </summary>
        public List<GroupDevicesDto> GroupDevices { get; set; }
        /// <summary>
        /// 设备分配
        /// </summary>
        public List<EnterpriseDevicesDto> EnterpriseDevices { get; set; }
        /// <summary>
        /// 设备服务商
        /// </summary>
        public List<DeviceServiceProvidersDto> DeviceServiceProviders { get; set; }
        /// <summary>
        /// 设置参数
        /// </summary>
        public SettingInfoDto SettingInfo { get; set; }
        /// <summary>
        /// 广告配置
        /// </summary>
        public AdvertisementInfoDto AdvertisementInfo { get; set; }
        /// <summary>
        /// 预警配置
        /// </summary>
        public EarlyWarningConfigDto EarlyWarningConfig { get; set; }
        /// <summary>
        /// 饮品集合
        /// </summary>
        [NotMapped]
        public List<BeverageInfoDto> BeverageInfos { get; set; }

        /// <summary>
        /// 激活时间
        /// </summary>
        public DateTime? ActiveTime { get; set; }

        /// <summary>
        /// 制作次数
        /// </summary>
        public int MakeCount { get; set; } = 0;
    }

    /// <summary>
    /// H5设备列表
    /// </summary>
    public class DeviceH5Dto
    {
        /// <summary>
        /// 设备
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 设备主键id
        /// </summary>
        public long? DeviceId { get; set; }

        /// <summary>
        /// 设备baseid
        /// </summary>
        public long? DeviceBaseId { get; set; }

        /// <summary>
        /// 设备通讯编码
        /// </summary>
        public string Mid { get; set; }

        /// <summary>
        /// 生产编号
        /// </summary>
        public string DeviceCode { get; set; }

        /// <summary>
        /// 是否缺货
        /// </summary>
        public bool IsQh { get; set; } = false;

        /// <summary>
        /// 是否在线
        /// </summary>
        public bool IsOnline { get; set; }
    }

    /// <summary>
    /// 设备信息集合
    /// </summary>
    public class DeviceInfoListDto
    {
        /// <summary>
        /// 设备信息集合
        /// </summary>
        public List<DeviceInfoDto> DeviceInfoList { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public DeviceInfoListDto() { DeviceInfoList = new List<DeviceInfoDto>(); }
    }

    /// <summary>
    /// 简约设备信息
    /// </summary>
    public class MiniDeviceInfoDto
    {
        /// <summary>
        /// 设备表Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// BaseInfoId
        /// </summary>
        public long DeviceBaseId { get; set; }
        /// <summary>
        /// 设备租户Id
        /// </summary>
        public long TenantId { get; set; }
    }
}