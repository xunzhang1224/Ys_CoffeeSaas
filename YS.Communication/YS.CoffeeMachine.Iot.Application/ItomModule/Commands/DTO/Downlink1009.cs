namespace YS.CoffeeMachine.Iot.Application.ItomModule.Commands.DTO
{
    using System;

    /// <summary>
    /// 下行指令 1009 的数据模型。
    /// 用于向IoT设备发送设备能力信息，包括硬件与软件支持的功能。
    /// </summary>
    public class Downlink1009
    {
        /// <summary>
        /// 获取或设置机器编号（售货机唯一标识）。
        /// </summary>
        public string VendCode { get; set; }

        /// <summary>
        /// 获取或设置硬件支持的功能位掩码。
        /// 每一位表示一种硬件功能是否启用。
        /// </summary>
        public int HardwareCapability { get; set; }

        /// <summary>
        /// 获取或设置软件支持的功能位掩码。
        /// 每一位表示一种软件功能是否启用。
        /// </summary>
        public int SoftwareCapability { get; set; }
    }
}