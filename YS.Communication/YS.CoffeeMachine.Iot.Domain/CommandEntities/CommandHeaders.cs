namespace YS.CoffeeMachine.Iot.Domain.CommandEntities
{

    /// <summary>
    /// command头部信息
    /// </summary>
    public class CommandHeaders
    {
        /// <summary>
        /// 指令号
        /// </summary>
        public int Key { get; set; }

        /// <summary>
        /// 机器编号
        /// </summary>
        public string Mid { get; set; }

        /// <summary>
        /// 消息ID
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public int Version { get; set; }
    }
}
