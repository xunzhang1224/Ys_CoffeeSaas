namespace YS.CoffeeMachine.Application.Dtos.IotCardDtos
{
    /// <summary>
    /// 请求返回类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HttpLotCardResult<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool? Succeeded { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public DataResult<T>? Data { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public long? Timestamp { get; set; }
    }

    /// <summary>
    /// 数据结果类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataResult<T>
    {
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string? Msg { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool? Success { get; set; }
    }

    /// <summary>
    /// 流量卡单条结果返回类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HttpLotCardSingleResult<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool? Succeeded { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public long? Timestamp { get; set; }
    }
}