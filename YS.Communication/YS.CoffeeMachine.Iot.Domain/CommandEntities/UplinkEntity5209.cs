namespace YS.CoffeeMachine.Iot.Domain.CommandEntities;
using MessagePack;

/// <summary>
/// 5209.机器补货上报
/// </summary>
public class UplinkEntity5209
{
    /// <summary>
    /// 指令号
    /// </summary>
    public static readonly int KEY = 5209;

    /// <summary>
    /// 请求
    /// </summary>
    [MessagePackObject(true)]
    public class Request : BaseCmd
    {

        /// <summary>
        /// 补货单号
        /// </summary>
        public string BizNo { get; set; }

        /// <summary>
        /// 该事务编号下的货道数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 货柜编号
        /// </summary>
        public int CounterNo { get; set; }

        /// <summary>
        /// 补货明细
        /// </summary>
        public IEnumerable<Detail> SlotInfo { get; set; }

        #region 嵌套结构

        /// <summary>
        /// 嵌套
        /// </summary>
        [MessagePackObject(true)]
        public class Detail
        {
            /// <summary>
            /// 货道号
            /// </summary>
            public int SlotNo { get; set; }

            /// <summary>
            /// 现存
            /// </summary>
            public decimal Stock { get; set; }

            /// <summary>
            /// 真实库存（蛇形机专用）
            /// </summary>
            public decimal RealStock { get; set; }

            /// <summary>
            /// 商品编码
            /// </summary>
            public string SKU { get; set; }

            /// <summary>
            /// 商品单价
            /// </summary>
            public decimal Price { get; set; }

            /// <summary>
            /// 货道容量
            /// </summary>
            public decimal Capacity { get; set; }
        }
        #endregion
    }

    /// <summary>
    /// 响应
    /// </summary>
    [MessagePackObject(true)]
    public class Response : BaseCmd
    {

        /// <summary>
        /// 补货单号
        /// </summary>
        public string BizNo { get; set; }

        /// <summary>
        /// 该事务编号下的货道数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 补货明细
        /// </summary>
        public IEnumerable<Detail> SlotInfo { get; set; }

        #region 嵌套结构

        /// <summary>
        /// 嵌套
        /// </summary>
        [MessagePackObject(true)]
        public class Detail
        {
            /// <summary>
            /// 货道号
            /// </summary>
            public int SlotNo { get; set; }

            /// <summary>
            /// 现存
            /// </summary>
            public decimal Stock { get; set; }

            /// <summary>
            /// 真实库存（蛇形机专用）
            /// </summary>
            public decimal RealStock { get; set; }

            /// <summary>
            /// 商品编码
            /// </summary>
            public string SKU { get; set; }

            /// <summary>
            /// 商品单价
            /// </summary>
            public decimal Price { get; set; }

            /// <summary>
            /// 货道容量
            /// </summary>
            public decimal Capacity { get; set; }
        }
        #endregion
    }
}
