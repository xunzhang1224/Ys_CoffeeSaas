namespace YS.Application.IoT.Service.Http
{
    using YS.Application.IoT.Service.Http.Dto;
    using YS.Domain.IoT.Model;

    /// <summary>
    /// HTTP 服务接口，用于与远程 HTTP API 交互。
    /// 提供设备状态上报、密钥获取等基础功能。
    /// </summary>
    public interface IHttp
    {
        /// <summary>
        /// 设置售货机在线状态。
        /// </summary>
        /// <param name="input">包含售货机编号和在线状态的输入参数。</param>
        /// <returns>异步操作结果，表示是否成功设置状态。</returns>
        Task<bool> SetVendLineStatus(VendLineStatusInput input);

        /// <summary>
        /// 获取指定设备的 MQTT 连接信息（如密钥、证书等敏感数据）。
        /// </summary>
        /// <param name="mid">设备唯一标识。</param>
        /// <returns>异步操作返回 VendInfoForMqttDto 对象，包含连接所需信息。</returns>
        Task<VendInfoForMqttDto> GetVendSecret(string mid);

        /// <summary>
        /// 获取指定设备的私钥信息。
        /// </summary>
        /// <param name="mid">设备唯一标识。</param>
        /// <returns>异步操作返回 VendToken 对象，包含私钥数据。</returns>
        Task<VendToken> GetPrivKey(string mid);

        /// <summary>
        /// 测试 HTTP 连通性。
        /// </summary>
        /// <returns>异步操作返回测试字符串，用于验证 HTTP 服务是否可用。</returns>
        Task<string> TestAsync();
    }
}