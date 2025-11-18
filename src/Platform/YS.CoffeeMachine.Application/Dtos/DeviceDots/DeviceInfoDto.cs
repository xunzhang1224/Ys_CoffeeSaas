using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.AdvertisementDtos;
using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YS.CoffeeMachine.Application.Dtos.SettringDtos;
using YS.CoffeeMachine.Application.Tools;
using YS.CoffeeMachine.Domain.AggregatesModel.Advertisements;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;

namespace YS.CoffeeMachine.Application.Dtos.DeviceDots
{
    /// <summary>
    /// 设备信息dto
    /// </summary>
    public class DeviceInfoDto
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; }

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
        /// 设备类型text
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
        /// 设备分组文本
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
        /// 国家地区id
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
        /// 设备状态
        /// </summary>
        public DeviceStatusEnum DeviceStatus { get; set; }

        /// <summary>
        /// 设备状态文本
        /// </summary>
        public string DeviceStatusText { get { return EnumExtensions.GetDescriptionsByInt<DeviceStatusEnum>((int)DeviceStatus); } }
        /// <summary>
        /// 使用场景
        /// </summary>
        public UsageScenarioEnum? UsageScenario { get; set; }
        /// <summary>
        /// POS机编号
        /// </summary>
        public string POSMachineNumber { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建时间文本
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
    }

    /// <summary>
    /// DeviceInfoListDto
    /// </summary>
    public class DeviceInfoListDto
    {
        /// <summary>
        /// DeviceInfoList
        /// </summary>
        public List<DeviceInfoDto> DeviceInfoList { get; set; }

        /// <summary>
        /// DeviceInfoListDto
        /// </summary>
        public DeviceInfoListDto() { DeviceInfoList = new List<DeviceInfoDto>(); }
    }
}