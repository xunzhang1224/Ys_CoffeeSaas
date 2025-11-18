namespace YS.CoffeeMachine.Iot.Domain.CommandEntities;
using MessagePack;

/// <summary>
/// UplinkEntity8889
/// </summary>
public class UplinkEntity8889
{
    /// <summary>
    /// 请求
    /// </summary>
    [MessagePackObject(true)]
    public class Request : BaseCmd
    {

        /// <summary>
        /// 订单号
        /// </summary>
        public string? OrderNo { get; set; }

        /// <summary>
        /// 响应
        /// </summary>
        [MessagePackObject(true)]
        public class Response : BaseCmd
        {
            /// <summary>
            /// 订单号
            /// </summary>
            public string? OrderNo { get; set; }

            /// <summary>
            /// 完成成功
            /// </summary>
            public bool? Successed { get; set; }
        }
    }
}
