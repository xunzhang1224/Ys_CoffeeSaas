namespace YS.CoffeeMachine.Iot.Domain.CommandEntities
{
    using MessagePack;

    /// <summary>
    /// 7212.交易上报（外部）
    /// </summary>
    public class UplinkEntity7212
    {
        /// <summary>
        /// 指令号
        /// </summary>
        public static readonly int KEY = 7212;

        /// <summary>
        /// 请求
        /// </summary>
        [MessagePackObject(true)]
        public class Request : BaseCmd
        {
            /// <summary>
            /// 是否订单数据
            /// </summary>
            public bool IsOrder { get; set; }

            /// <summary>
            /// 机器本地订单编号
            /// </summary>
            public string BizNo { get; set; }

            /// <summary>
            /// 订单明细
            /// </summary>
            public List<DetailInfo> Details { get; set; }

            /// <summary>
            /// 支付方式信息
            /// </summary>
            public PaymentInfo Payment { get; set; }

            #region 嵌套结构

            /// <summary>
            /// 嵌套
            /// </summary>
            [MessagePackObject(true)]
            public class DetailInfo
            {
                /// <summary>
                /// 货柜编号
                /// </summary>
                public int CounterNo { get; set; }

                /// <summary>
                /// 货道编号
                /// </summary>
                public int SlotNo { get; set; }

                /// <summary>
                /// 商品编码
                /// </summary>
                public string ItemCode { get; set; }

                /// <summary>
                /// 数量
                /// </summary>
                public int Quantity { get; set; }

                /// <summary>
                /// 价格
                /// </summary>
                public decimal Price { get; set; }

                /// <summary>
                /// 出货信息
                /// </summary>
                public DeliveryInfo Delivery { get; set; }

                /// <summary>
                /// 物料消耗
                /// </summary>
                public List<Material> Materials { get; set; }
            }

            /// <summary>
            /// 物料
            /// </summary>
            [MessagePackObject(true)]
            public class Material
            {
                /// <summary>
                /// 物料类型
                /// </summary>
                public int Type { get; set; }

                /// <summary>
                /// 序号
                /// </summary>
                public int Index { get; set; }

                /// <summary>
                /// 使用量
                /// </summary>
                public int Value { get; set; }
            }

            /// <summary>
            /// 出货信息
            /// </summary>
            [MessagePackObject(true)]
            public class DeliveryInfo
            {
                /// <summary>
                /// 1 出货成功 0-出货失败
                /// </summary>
                public int Result { get; set; }

                /// <summary>
                /// 错误
                /// </summary>
                public int Error { get; set; }

                /// <summary>
                /// 错误描述
                /// </summary>
                public string ErrorDescription { get; set; }

                /// <summary>
                /// 出货时间
                /// </summary>
                public long ActionTimeSp { get; set; }
            }

            /// <summary>
            /// 支付信息
            /// </summary>
            [MessagePackObject(true)]
            public class PaymentInfo
            {
                /// <summary>
                /// 货币代码
                /// </summary>
                public string CurrencyCode { get; set; }

                /// <summary>
                /// 消费金额(元)
                /// </summary>
                public decimal Amount { get; set; }

                /// <summary>
                /// 付款方案
                /// </summary>
                public string Provider { get; set; }

                /// <summary>
                /// 支付时间
                /// </summary>
                public long PayTimeSp { get; set; }
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
            /// 状态：0.成功 1.失败
            /// </summary>
            public int Status { get; set; }

            /// <summary>
            /// 机器本地订单编号
            /// </summary>
            public string BizNo { get; set; }

            /// <summary>
            /// 返回服务器生成的订单号
            /// </summary>
            public string OrderNo { get; set; }

            /// <summary>
            /// 描叙，比如失败的原因
            /// </summary>
            public string Description { get; set; }
        }
    }
}
