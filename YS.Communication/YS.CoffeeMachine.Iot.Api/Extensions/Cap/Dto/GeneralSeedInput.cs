namespace YS.CoffeeMachine.Iot.Api.Extensions.Cap.Dto
{
    /// <summary>
    /// 下发通用入参
    /// </summary>
    public class GeneralSeedInput: GeneralSeedBase
    {
        /// <summary>
        /// 根据method转json
        /// </summary>
        public string Params { get; set; }
    }

    /// <summary>
    /// GeneralSeedBase
    /// </summary>
    public class GeneralSeedBase
    {
        /// <summary>
        /// 机器编号
        /// </summary>
        public string Mid { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public long TimeSp { get; set; } = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

        /// <summary>
        /// 服务/命令 唯一标识
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 消息唯一id
        /// </summary>
        public string TransId { get; set; }
    }
}
