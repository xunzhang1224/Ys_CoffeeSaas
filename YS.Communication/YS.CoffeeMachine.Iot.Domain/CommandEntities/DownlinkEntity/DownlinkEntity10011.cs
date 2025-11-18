namespace YS.CoffeeMachine.Iot.Domain.CommandEntities.DownlinkEntity
{
    using MessagePack;

    /// <summary>
    /// 10011(机器播报语音)
    /// </summary>
    public class DownlinkEntity10011
    {
        /// <summary>
        /// 指令号
        /// </summary>
        public static readonly int KEY = 10011;

        /// <summary>
        /// 请求
        /// </summary>
        [MessagePackObject(true)]
        public class Request : BaseCmd
        {
            /// <summary>
            /// 语音消息
            /// </summary>
            public string Msg { get; set; }
        }
    }
}
