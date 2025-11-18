namespace YS.CoffeeMachine.Iot.Domain.CommandEntities;
using MessagePack;
using static YS.CoffeeMachine.Iot.Domain.CommandEntities.UplinkEntity7212.Request;

/// <summary>
/// 用于4201号指令的协议实体：VMC向服务器上报出货结果(内部).
/// </summary>
public class UplinkEntity4201
{

    /// <summary>
    /// 指令号
    /// </summary>
    public static readonly int KEY = 4201;

    /// <summary>
    /// 请求
    /// </summary>
    [MessagePackObject(true)]
    public class Request : BaseCmd
    {
        /// <summary>
        /// TransId
        /// </summary>
        public string TransId { get; set; }

        /// <summary>
        /// 订单号.
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 出货明细.
        /// </summary>
        public IEnumerable<Order> Orders { get; set; }

        #region 嵌套结构

        /// <summary>
        /// 嵌套
        /// </summary>
        [MessagePackObject(true)]
        public class Order
        {
            /// <summary>
            /// 订单明细子订单号
            /// </summary>
            public string SubOrderNo { get; set; }

            /// <summary>
            /// 出货记录编号
            /// </summary>
            public string DeliveryId { get; set; }

            /// <summary>
            /// 物料消耗
            /// </summary>
            public List<Material> Materials { get; set; }

            /// <summary>
            /// 货道编号
            /// </summary>
            public int Slot { get; set; }

            /// <summary>
            /// 出货结果是否成功
            /// </summary>
            public int Result { get; set; }

            /// <summary>
            /// 错误
            /// </summary>
            public int Error { get; set; }

            /// <summary>
            /// 错误描叙
            /// </summary>
            public string ErrorDescription { get; set; }

            /// <summary>
            /// 出货时间
            /// </summary>
            public long ActionTimeSp { get; set; }

            /// <summary>
            /// 货柜编号
            /// </summary>
            public int CounterNo { get; set; }
        }
        #endregion
    }

    /// <summary>
    /// 应答实体.
    /// </summary>
    [MessagePackObject(true)]
    public class Response : BaseCmd
    {
        /// <summary>
        /// TransId
        /// </summary>
        public string TransId { get; set; }

        /// <summary>
        /// 订单号.
        /// </summary>
        public string OrderNo { get; set; }
    }
}