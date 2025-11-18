namespace YS.CoffeeMachine.Iot.Domain.CommandEntities;
using MessagePack;

/// <summary>
/// 用于5205号指令的请求实体：VMC向服务器上报货道（机器货道同步）.
/// </summary>
public class UplinkEntity5205
{
    /// <summary>
    /// 指令号
    /// </summary>
    public static readonly int KEY = 5205;

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
        /// Gets or sets 货柜编号
        /// </summary>
        public int CounterNo { get; set; }

        /// <summary>
        /// Gets or sets 货道总数目.
        /// </summary>
        public int MaxSlot { get; set; }

        /// <summary>
        /// Gets or sets 货道信息集.
        /// </summary>
        public List<SlotEntity> SlotInfo { get; set; }

        #region 嵌套结构

        /// <summary>
        /// 表示货道信息的实体.
        /// </summary>
        [MessagePackObject(true)]
        public class SlotEntity
        {
            /// <summary>
            /// 货道号。
            /// </summary>
            public int SlotNo { get; set; }

            /// <summary>
            /// 层号。
            /// </summary>
            public int LayerNo { get; set; }

            /// <summary>
            /// 单价。
            /// </summary>
            public decimal Price { get; set; }

            /// <summary>
            /// 货道容量。
            /// </summary>
            public decimal Capacity { get; set; }

            /// <summary>
            /// 库存。
            /// </summary>
            public decimal Stock { get; set; }

            /// <summary>
            /// 折扣值，百分比1-100。
            /// </summary>
            public int Discount { get; set; }

            /// <summary>
            /// 加热时间长度(单位:秒),默认0。
            /// </summary>
            public int HeatingTime { get; set; } = 0;

            /// <summary>
            /// 真实库存（蛇形机专用）。
            /// </summary>
            public int RealStock { get; set; }

            /// <summary>
            /// 商品编码(仅安卓机型支持)
            /// </summary>
            public string SKU { get; set; }

            /// <summary>
            /// 状态:正常=1；故障=2；停用=3；缺货=4 不存在 = 255
            /// </summary>
            public int Status { get; set; }

            /// <summary>
            /// 扩展字段。
            /// </summary>
            public string Extra { get; set; }

            /// <summary>
            /// 商品过期时间戳，以秒为单位。如果为0，则是没有过期时间。
            /// </summary>
            public int ExpireTimeStamp { get; set; }
        }
        #endregion
    }

    /// <summary>
    /// 用于5205号指令的响应实体：VMC向服务器上报货道（机器货道同步）.
    /// </summary>
    [MessagePackObject(true)]
    public class Response : BaseCmd
    {
        /// <summary>
        /// TransId
        /// </summary>
        public string TransId { get; set; }

        /// <summary>
        /// Gets or sets 货柜编号.
        /// </summary>
        public int CounterNo { get; set; }

        /// <summary>
        /// Gets or sets 货道总数目.
        /// </summary>
        public int MaxSlot { get; set; }

        /// <summary>
        /// Gets or sets 货道信息集.
        /// </summary>
        public List<SlotEntity> SlotInfo { get; set; }

        #region 嵌套结构

        /// <summary>
        /// 表示货道信息的实体.
        /// </summary>
        [MessagePackObject(true)]
        public class SlotEntity
        {
            /// <summary>
            /// Gets or sets 货道编号.
            /// </summary>
            public int SlotNo { get; set; }
        }
        #endregion
    }
}