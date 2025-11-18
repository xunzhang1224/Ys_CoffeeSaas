namespace YS.CoffeeMachine.Iot.Domain.CommandEntities
{
    using MessagePack;

    /// <summary>
    /// 7214.VMC取货码请求取货
    /// </summary>
    public class UplinkEntity7214
    {
        /// <summary>
        /// 指令号
        /// </summary>
        public static readonly int KEY = 7214;

        /// <summary>
        /// 请求
        /// </summary>
        [MessagePackObject(true)]
        public class Request : BaseCmd
        {

            /// <summary>
            /// 取货码编码
            /// </summary>
            public string PickupCode { get; set; }
        }

        /// <summary>
        /// 接收
        /// </summary>
        [MessagePackObject(true)]
        public class Response : BaseCmd
        {

            /// <summary>
            /// 0 申请成功 1.取货码无效
            /// </summary>
            public int Result { get; set; }

            /// <summary>
            /// 返回描叙信息
            /// </summary>
            public string Description { get; set; }
        }
    }
}
