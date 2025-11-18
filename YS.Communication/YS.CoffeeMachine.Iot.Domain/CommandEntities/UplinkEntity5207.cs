namespace YS.CoffeeMachine.Iot.Domain.CommandEntities
{
    using MessagePack;

    /// <summary>
    /// 提供5207.VMC向服务器上报货道（仅货道结构）的请求实体
    /// </summary>
    public class UplinkEntity5207
    {
        /// <summary>
        /// 指令号
        /// </summary>
        public static readonly int KEY = 5207;

        /// <summary>
        /// 请求
        /// </summary>
        [MessagePackObject(true)]
        public class Request : BaseCmd
        {
            #region 公共属性

            /// <summary>
            /// TransId
            /// </summary>
            public string TransId { get; set; }

            /// <summary>
            /// 货柜编号
            /// </summary>
            public int CounterNo { get; set; }

            /// <summary>
            /// 货道总数目
            /// </summary>
            public int MaxSlot { get; set; }

            /// <summary>
            /// 货道信息集
            /// </summary>
            public List<SlotEntity> SlotInfo { get; set; }
            #endregion

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
            }
            #endregion
        }

        /// <summary>
        /// 响应
        /// </summary>
        [MessagePackObject(true)]
        public class Response : BaseCmd
        {
            #region 公共属性

            /// <summary>
            /// TransId
            /// </summary>
            public string TransId { get; set; }

            /// <summary>
            /// 货柜编号
            /// </summary>
            public int CounterNo { get; set; }

            /// <summary>
            /// 货道总数目
            /// </summary>
            public int MaxSlot { get; set; }

            /// <summary>
            /// 货道信息集
            /// </summary>
            public List<SlotEntity> SlotInfo { get; set; }
            #endregion

            #region 嵌套结构

            /// <summary>
            /// 表示货道信息的实体.
            /// </summary>
            [MessagePackObject(true)]
            public class SlotEntity
            {
                /// <summary>
                /// 货道编号
                /// </summary>
                public int SlotNo { get; set; }
            }
            #endregion
        }
    }
}