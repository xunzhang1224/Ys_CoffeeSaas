namespace YS.Application.IoT.Service.Http.Dto
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// 设备注册请求参数。
    /// 用于设备首次连接时向服务器提交基础信息。
    /// </summary>
    public class RegisterDeviceInput
    {
        // 注意：MID 字段已被注释，当前未使用

        /// <summary>
        /// 设备的唯一国际移动设备识别码。
        /// </summary>
        public string IMEI { get; set; }

        /// <summary>
        /// 设备公钥，用于安全通信的身份验证。
        /// </summary>
        public string PubKey { get; set; }

        /// <summary>
        /// 时间戳，单位为秒。
        /// 通常用于对时或防止重放攻击。
        /// </summary>
        public long TimeSp { get; set; }

        /// <summary>
        /// 通道 ID，默认值为 1。
        /// 可用于区分不同的接入通道或业务类型。
        /// </summary>
        public int ChannelId { get; set; } = 1;
    }

    /// <summary>
    /// 售货机在线状态更新请求参数。
    /// 用于上报售货机当前是否在线。
    /// </summary>
    public class VendLineStatusInput
    {
        /// <summary>
        /// 售货机编号，唯一标识一台设备。
        /// </summary>
        public string VendCode { get; set; }

        /// <summary>
        /// 在线状态。
        /// false 表示离线，true 表示在线。
        /// </summary>
        public bool LineStatus { get; set; }
    }
}