namespace YS.CoffeeMachine.Iot.Api.Dto
{
    /// <summary>
    /// 下发日志实体
    /// </summary>
    public class DeviceLogDto
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public long StartTimestamp { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public long EndTimestamp { get; set; }
    }
}
