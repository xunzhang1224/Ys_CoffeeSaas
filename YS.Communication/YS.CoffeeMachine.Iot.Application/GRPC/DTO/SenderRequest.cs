namespace YS.CoffeeMachine.Iot.Application.GRPC.DTO
{
    using MessagePack;

    /// <summary>
    /// 表示发送到IoT设备或服务的请求数据模型。
    /// 用于gRPC通信中的消息体封装，支持MessagePack序列化。
    /// </summary>
    [MessagePackObject(true)]
    public class SenderRequest
    {
        /// <summary>
        /// 获取或设置请求的业务标识Key，用于区分不同的操作类型或模块。
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 获取或设置消息目标设备的唯一标识（Machine ID）。
        /// </summary>
        public string Mid { get; set; }

        /// <summary>
        /// 获取或设置请求的时间戳（Unix时间戳格式，单位为秒或毫秒）。
        /// 用于消息时效性判断及去重处理。
        /// </summary>
        public long TimeSp { get; set; }

        /// <summary>
        /// 获取或设置当前请求的唯一消息ID，用于日志追踪与幂等处理。
        /// 默认为空字符串。
        /// </summary>
        public string MessageId { get; set; } = string.Empty;

        /// <summary>
        /// 获取或设置请求体内容，通常为JSON字符串或其他序列化数据。
        /// 默认为空字符串。
        /// </summary>
        public string Entity { get; set; } = string.Empty;
    }
}