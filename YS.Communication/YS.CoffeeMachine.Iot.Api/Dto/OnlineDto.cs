namespace YS.CoffeeMachine.Iot.Api.Dto
{
    /// <summary>
    /// 设备在线情况dto
    /// </summary>
    public class OnlineDto
    {
        /// <summary>
        /// 状态
        /// </summary>
        public string Mid { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime OffDate { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime OnDate { get; set; }
    }
}
