namespace YS.CoffeeMachine.Iot.Domain.CommandEntities;
using MessagePack;

/// <summary>
/// 5206.VMC向服务器上报故障清除的实体
/// </summary>
public class UplinkEntity5206
{
    /// <summary>
    /// 指令号
    /// </summary>
    public static readonly int KEY = 5206;

    /// <summary>
    /// 请求
    /// </summary>
    [MessagePackObject(true)]
    public class Request : BaseCmd
    {
        /// <summary>
        /// Details
        /// </summary>
        public IEnumerable<Detail> Details { get; set; }

        /// <summary>
        /// Details
        /// </summary>
        [MessagePackObject(true)]
        public class Detail
        {
            /// <summary>
            /// 货道号
            /// </summary>
            public int SlotNo { get; set; }
        }
    }

    /// <summary>
    /// 响应
    /// </summary>
    [MessagePackObject(true)]
    public class Response : BaseCmd
    {
    }
}
