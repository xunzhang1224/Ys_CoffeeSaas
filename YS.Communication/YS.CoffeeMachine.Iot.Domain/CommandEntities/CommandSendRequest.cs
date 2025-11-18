namespace YS.CoffeeMachine.Iot.Domain.CommandEntities
{
    /// <summary>
    /// command发送请求
    /// </summary>
    public class CommandSendRequest
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
        public string? MessageId { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public int Version { get; set; } = 1;

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
    }
}
