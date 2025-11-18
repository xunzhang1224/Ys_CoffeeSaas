namespace YS.CoffeeMachine.Domain.AggregatesModel.Devices
{
    using System.ComponentModel.DataAnnotations;
    using YS.CoffeeMachine.Domain.Shared.Enum;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 表示设备硬件信息的实体类。
    /// 该类继承自 BaseEntity，用于存储与设备相关的硬件组件信息。
    /// 主要用于记录设备中各个硬件的状态、类型、错误信息等元数据。
    /// </summary>
    public class DeviceHardwares : BaseEntity
    {
        /// <summary>
        /// 获取或设置关联的设备基础信息唯一标识符。
        /// 用于标识此硬件属于哪个设备。
        /// </summary>
        [Required]
        public long DeviceBaseId { get; set; }

        /// <summary>
        /// 获取或设置硬件的唯一编号，用于程序识别和匹配。
        /// </summary>
        [Required]
        public string Code { get; private set; }

        /// <summary>
        /// 获取或设置硬件的可读名称，用于展示给用户或前端界面。
        /// 可为空。
        /// </summary>
        public string? HardwareName { get; private set; }

        /// <summary>
        /// 获取或设置硬件的类型，默认值为 Material（物料类）。
        /// 其他可能类型参考 HardwareTypeEnum 定义。
        /// </summary>
        public HardwareTypeEnum HardwareType { get; private set; } = HardwareTypeEnum.Material;

        /// <summary>
        /// 获取或设置硬件对应的图片路径（URL），用于前端显示图标或图示。
        /// 可为空。
        /// </summary>
        public string? PictureUrl { get; private set; }

        /// <summary>
        /// 获取或设置当前硬件的状态（是否正常运行）。
        /// 默认值为 false（表示异常或未就绪）。
        /// </summary>
        public bool Status { get; private set; } = false;

        /// <summary>
        /// 获取或设置当前硬件的错误信息（如果状态为异常）。
        /// 可为空。
        /// </summary>
        public string? Message { get; private set; }

        /// <summary>
        /// 获取与此硬件关联的设备基础信息对象。
        /// 用于导航至父级设备信息。
        /// </summary>
        public DeviceBaseInfo DeviceBaseInfo { get; }

        /// <summary>
        /// 受保护的无参构造函数，供ORM工具使用。
        /// </summary>
        protected DeviceHardwares() { }

        /// <summary>
        /// 使用指定参数初始化一个新的 DeviceHardwares 实例。
        /// </summary>
        /// <param name="code">硬件的唯一编号。</param>
        /// <param name="hardwareName">硬件的可读名称。</param>
        /// <param name="hardwareType">硬件的类型。</param>
        /// <param name="pictureUrl">硬件的图片路径。</param>
        /// <param name="status">硬件的当前状态。</param>
        /// <param name="message">硬件的错误信息。</param>
        /// <param name="deviceId">关联的设备基础信息唯一标识。</param>
        public DeviceHardwares(string code, string hardwareName, HardwareTypeEnum hardwareType, string pictureUrl, bool status, string message, long deviceId)
        {
            Code = code;
            HardwareName = hardwareName;
            PictureUrl = pictureUrl;
            HardwareType = hardwareType;
            Status = status;
            DeviceBaseId = deviceId;
            Message = message;
        }

        /// <summary>
        /// 更新硬件的状态及错误信息。
        /// 如果状态为正常，则清空错误信息；否则保留传入的错误信息。
        /// </summary>
        /// <param name="status">新的状态值。</param>
        /// <param name="message">新的错误信息。</param>
        public void UpdateStatus(bool status, string message)
        {
            Status = status;
            Message = status ? "" : message;
        }
    }
}