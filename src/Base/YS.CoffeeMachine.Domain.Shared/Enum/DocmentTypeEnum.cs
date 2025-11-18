using System.ComponentModel;

namespace YS.CoffeeMachine.Domain.Shared.Enum
{
    /// <summary>
    /// 导入的文件类型
    /// </summary>
    [Description("导入的文件类型")]
    public enum DocmentTypeEnum
    {
        /// <summary>
        /// 多语言
        /// </summary>
        [Description("多语言")]
        Language,

        /// <summary>
        /// 日志上报
        /// </summary>
        [Description("日志上报")]
        LogUpload,

        /// <summary>
        /// 设备上下线
        /// </summary>
        [Description("设备上下线")]
        DeviceOnline,

        /// <summary>
        /// 设备事件
        /// </summary>
        [Description("设备事件")]
        DeviceEvent,

        /// <summary>
        /// 设备异常
        /// </summary>
        [Description("设备异常")]
        DeviceError,

        /// <summary>
        /// 售卖记录
        /// </summary>
        [Description("售卖记录")]
        OrderLog,
    }
}
