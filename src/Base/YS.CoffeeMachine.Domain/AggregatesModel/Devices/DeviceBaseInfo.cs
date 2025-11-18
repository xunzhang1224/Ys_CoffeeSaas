namespace YS.CoffeeMachine.Domain.AggregatesModel.Devices
{
    using Aop.Api.Domain;
    using System.ComponentModel.DataAnnotations;
    using YS.CoffeeMachine.Domain.Events.BasicDomainEvents.Language;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 表示设备基础信息的聚合根实体。
    /// 该类继承自 BaseEntity，并实现了 IAggregateRoot 接口，表明其在领域模型中作为聚合根的角色。
    /// 主要用于存储和管理设备的基本信息，包括通讯编码、硬件信息、软件状态、网络配置等。
    /// </summary>
    public class DeviceBaseInfo : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 获取或设置设备的通讯编码（MID），用于设备与平台之间的通信标识。
        /// </summary>
        public string? Mid { get; private set; }

        /// <summary>
        /// 获取或设置设备的车贴码/生产编号，用于物理设备的唯一标识。
        /// </summary>
        [Required]
        public string MachineStickerCode { get; private set; }

        /// <summary>
        /// 获取或设置设备所属箱体的唯一标识符。
        /// </summary>
        [Required]
        public string BoxId { get; private set; }

        /// <summary>
        /// 获取或设置设备型号的唯一标识，可为空。
        /// </summary>
        public long? DeviceModelId { get; private set; }

        /// <summary>
        /// 获取或设置设备是否已出厂的状态，默认为已出厂。
        /// </summary>
        public IsLeaveFactoryEnum IsLeaveFactory { get; private set; } = IsLeaveFactoryEnum.Yes;

        /// <summary>
        /// 获取或设置设备连接的 WIFI 网络名称（SSID），可为空。
        /// </summary>
        public string? SSID { get; private set; }

        /// <summary>
        /// 获取或设置设备的硬件能力标识，用于描述硬件功能集，可为空。
        /// </summary>
        public int? HardwareCapability { get; private set; }

        /// <summary>
        /// 获取或设置设备的软件能力标识，用于描述软件功能集，可为空。
        /// </summary>
        public int? SoftwareCapability { get; private set; }

        /// <summary>
        /// 获取或设置设备的 MAC 地址，用于唯一标识网络设备，可为空。
        /// </summary>
        public string? MAC { get; private set; }

        /// <summary>
        /// 获取或设置主程序的版本号，用于跟踪当前运行的固件版本，可为空。
        /// </summary>
        public string? VersionNumber { get; private set; }

        /// <summary>
        /// 获取或设置皮肤插件的版本号，用于界面主题更新，可为空。
        /// </summary>
        public string? SkinPluginVersion { get; private set; }

        /// <summary>
        /// 获取或设置语言包版本，用于支持多语言环境，可为空。
        /// </summary>
        public string? LanguagePack { get; private set; }

        /// <summary>
        /// 获取或设置软件最后一次更新的时间戳，UTC 时间。
        /// </summary>
        public DateTime? SoftwareUpdateLastTime { get; private set; }

        /// <summary>
        /// 获取当前设备是否在线的状态。
        /// </summary>
        public bool IsOnline { get; private set; }

        /// <summary>
        /// 获取或设置设备最近一次上线时间，UTC 时间。
        /// </summary>
        public DateTime? UpdateOnlineTime { get; private set; }

        /// <summary>
        /// 获取或设置设备最近一次下线时间，UTC 时间。
        /// </summary>
        public DateTime? UpdateOfflineTime { get; private set; }

        /// <summary>
        /// 获取或设置与此设备关联的硬件组件列表。
        /// </summary>
        public List<DeviceHardwares>? DeviceHardwares { get; private set; }

        // /// <summary>
        // /// 获取或设置设备的运行时指标数据，如温度、电压等。
        // /// </summary>
        // public List<DeviceMetrics>? DeviceMetrics { get; private set; }

        /// <summary>
        /// 受保护的无参构造函数，供ORM工具使用。
        /// </summary>
        protected DeviceBaseInfo()
        {
        }

        /// <summary>
        /// 使用指定参数初始化一个新的 DeviceBaseInfo 实例。
        /// </summary>
        /// <param name="mid">设备的通讯编码。</param>
        /// <param name="machineStickerCode">设备的车贴码/生产编号。</param>
        /// <param name="boxId">设备所属箱体的唯一标识。</param>
        public DeviceBaseInfo(string mid, string machineStickerCode, string boxId, long deviceModelId)
        {
            Mid = mid;
            MachineStickerCode = machineStickerCode;
            BoxId = boxId;
            DeviceModelId = deviceModelId;
        }

        /// <summary>
        /// 更新设备的软件相关信息。
        /// </summary>
        /// <param name="versionNumber">新的主程序版本号。</param>
        /// <param name="skinPluginVersion">新的皮肤插件版本。</param>
        /// <param name="languagePack">新的语言包版本。</param>
        public void UpdateSoftwareInfo(string versionNumber, string skinPluginVersion, string languagePack)
        {
            VersionNumber = versionNumber;
            SkinPluginVersion = skinPluginVersion;
            LanguagePack = languagePack;
            SoftwareUpdateLastTime = DateTime.UtcNow;
        }

        /// <summary>
        /// 绑定新的设备通讯编码（MID）。
        /// </summary>
        /// <param name="mid">新的设备通讯编码。</param>
        public void BindMid(string mid)
        {
            Mid = mid;
        }

        /// <summary>
        /// 解绑
        /// </summary>
        /// <param name="mid">新的设备通讯编码。</param>
        public void UnBindMid()
        {
            Mid = null;
        }

        /// <summary>
        /// 设置设备的硬件信息，合并已有硬件条目或新增新条目。
        /// </summary>
        /// <param name="deviceHardwares">要设置的硬件信息列表。</param>
        public void SetDeviceHardwares(List<DeviceHardwares> deviceHardwares)
        {
            foreach (DeviceHardwares item in deviceHardwares)
            {
                var old = DeviceHardwares.FirstOrDefault(x => x.Code == item.Code);
                if (old != null)
                    old.UpdateStatus(item.Status, item.Message);
                else
                    DeviceHardwares.Add(item);
            }
        }

        /// <summary>
        /// 将设备标记为在线状态，并记录上线时间。
        /// </summary>
        public void Online()
        {
            IsOnline = true;
            UpdateOnlineTime = DateTime.UtcNow;
        }

        /// <summary>
        /// 将设备标记为离线状态，并记录下线时间。
        /// </summary>
        public void Offline()
        {
            IsOnline = false;
            UpdateOfflineTime = DateTime.UtcNow;
        }

        /// <summary>
        /// 设置设备的硬件能力和软件能力。
        /// </summary>
        /// <param name="hardwareCapability">表示硬件能力的整数值。</param>
        /// <param name="softwareCapability">表示软件能力的整数值。</param>
        public void SetAbility(int hardwareCapability, int softwareCapability)
        {
            HardwareCapability = hardwareCapability;
            SoftwareCapability = softwareCapability;
        }
    }
}