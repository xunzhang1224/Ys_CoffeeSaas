using YS.CoffeeMachine.Domain.AggregatesModel.Basics;

namespace YS.CoffeeMachine.Domain.AggregatesModel.Devices
{
    using System.ComponentModel.DataAnnotations;
    using YS.CoffeeMachine.Domain.Shared.Enum;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 设备清洗部件记录
    /// </summary>
    public class DeviceFlushComponentsLog : EnterpriseBaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 获取或设置关联的平台设备唯一标识符。
        /// 用于标识该属性所属的设备基础信息。
        /// </summary>
        [Required]
        public long DeviceBaseId { get; set; }

        /// <summary>
        /// 生产编号
        /// </summary>
        [Required]
        public string MachineStickerCode { get; set; }

        /// <summary>
        /// 设备名字
        /// </summary>
        public string? DeviceName { get; private set; }

        /// <summary>
        /// 清洗类型
        /// 区分  搅拌器 冲泡器 料盒清洗
        /// </summary>
        [Required]
        public string Type { get; set; }

        /// <summary>
        /// 清洗模式
        /// </summary>
        [Required]
        public FlushComponentTypeEnum FlushType { get; set; }

        /// <summary>
        /// 部件
        /// </summary>
        [Required]
        public string Parts { get; set; }

        /// <summary>
        ///   1： 完成  2失败
        /// </summary>
        public int Status { get; set; } = 1;

        /// <summary>
        /// 构造函数
        /// </summary>
        protected DeviceFlushComponentsLog() { }

        /// <summary>
        /// 9032
        /// </summary>
        /// <param name="deviceBaseId"></param>
        /// <param name="machineStickerCode"></param>
        /// <param name="deviceName"></param>
        /// <param name="flushType"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="status"></param>
        public DeviceFlushComponentsLog(long deviceBaseId, string machineStickerCode, string deviceName, FlushComponentTypeEnum flushType, string type, string name, int status,long enterpriseinfoId)
        {
            DeviceBaseId = deviceBaseId;
            MachineStickerCode = machineStickerCode;
            DeviceName = deviceName;
            FlushType = flushType;
            Parts = name;
            Type = type;
            Status = status;
            EnterpriseinfoId = enterpriseinfoId;
        }
    }
}
