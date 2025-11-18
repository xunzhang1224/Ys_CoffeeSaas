namespace YS.CoffeeMachine.Iot.Domain.CommandEntities;
using MessagePack;

/// <summary>
/// UplinkEntity8888
/// </summary>
public class UplinkEntity8888
{
    /// <summary>
    /// 请求
    /// </summary>
    [MessagePackObject(true)]
    public class Request : BaseCmd
    {
        /// <summary>
        /// 第三方支付ID
        /// </summary>
        public string OpendId { get; set; }

        /// <summary>
        /// 订单类型。1-微信扫码 2-支付宝扫码
        /// </summary>
        public int OrderType { get; set; } = 1;

        /// <summary>
        /// 支付类型。1-微信 2-支付包
        /// </summary>
        public int PayType { get; set; } = 1;

        /// <summary>
        /// 选购明细。
        /// </summary>
        public List<OrderDetailDTO> Details { get; set; }

        /// <summary>
        /// 配置
        /// </summary>
        [MessagePackObject(true)]
        public class OrderDetailDTO
        {
            /// <summary>
            /// 货柜编号。默认为0
            /// </summary>
            public int CounterNo { get; set; }

            /// <summary>
            /// 商品ID
            /// </summary>
            public string? ItemId { get; set; }

            /// <summary>
            /// 货道编号
            /// </summary>
            public int SlotNo { get; set; }

            /// <summary>
            /// 数量
            /// </summary>
            public decimal Quantity { get; set; }
        }

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
        }
    }
}
