namespace YS.CoffeeMachine.Iot.Domain.CommandEntities
{
    using MessagePack;

    /// <summary>
    /// command发送结果
    /// </summary>
    [MessagePackObject(true)]
    public class CommandSendResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 消息ID
        /// </summary>
        public string MessageId { get; set; }
    }
}
