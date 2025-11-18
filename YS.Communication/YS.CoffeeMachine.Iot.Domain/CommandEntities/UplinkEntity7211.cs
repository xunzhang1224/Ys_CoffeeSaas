namespace YS.CoffeeMachine.Iot.Domain.CommandEntities;
using MessagePack;
using System.ComponentModel;

/// <summary>
/// 7211.交易上报
/// </summary>
public class UplinkEntity7211
{
    /// <summary>
    /// 指令号
    /// </summary>
    public static readonly int KEY = 7211;

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
        ///安卓上报订单号
        /// </summary>
        public string? OutTradeNo { get; set; }

        /// <summary>
        /// 出货方式
        /// </summary>
        public ShippingMethodEnum ShippingMethod { get; set; } = ShippingMethodEnum.Manually;

        /// <summary>
        /// 详情
        /// </summary>
        public IEnumerable<DetailEntity> Details { get; set; }

        /// <summary>
        /// 支付
        /// </summary>
        public PaymentEntity Payment { get; set; }

        /// <summary>
        /// 订单商品信息
        /// </summary>
        [MessagePackObject(true)]
        public class DetailEntity
        {
            /// <summary>
            /// 货柜号
            /// </summary>
            public int CounterNo { get; set; }

            /// <summary>
            /// 货道号
            /// </summary>
            public int SlotNo { get; set; }

            /// <summary>
            /// 货层
            /// </summary>
            public int LayerNo { get; set; }

            /// <summary>
            /// 商品条码
            /// </summary>
            public string ItemCode { get; set; }

            /// <summary>
            /// 商品条码
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 数量
            /// </summary>
            public int Quantity { get; set; }

            /// <summary>
            /// 金额
            /// </summary>
            public decimal Price { get; set; }
        }

        /// <summary>
        /// 支付方案
        /// </summary>
        [MessagePackObject(true)]
        public class PaymentEntity
        {
            /// <summary>
            /// 金额
            /// </summary>
            public decimal Amount { get; set; }

            /// <summary>
            /// 供应商
            /// </summary>
            public string Provider { get; set; }

            /// <summary>
            /// 方式
            /// </summary>
            public string Method { get; set; }

            /// <summary>
            /// Argument
            /// </summary>
            public string Argument { get; set; }

            /// <summary>
            /// Applet
            /// </summary>
            public string Applet { get; set; }

            /// <summary>
            /// 账号名字
            /// </summary>
            public string AccountName { get; set; }

            /// <summary>
            /// 账号code
            /// </summary>
            public string AccountCode { get; set; }

            /// <summary>
            /// 账号类型
            /// </summary>
            public string AccountType { get; set; }
        }
    }

    /// <summary>
    /// 响应
    /// </summary>
    [MessagePackObject(true)]
    public class Response : BaseCmd
    {
        /// <summary>
        /// TransId
        /// </summary>
        public string TransId { get; set; }

        /// <summary>
        /// 状态：0.成功 1.失败
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 消费金额(元)
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 结果：如二维码需要生成的内容
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 描述，比如失败的原因
        /// </summary>
        public string Description { get; set; }
    }
}

/// <summary>
/// 出货方式
/// </summary>
[Description("出货方式")]
public enum ShippingMethodEnum
{
    /// <summary>
    /// 手动
    /// </summary>
    [Description("手动")]
    Manually = 1,
    /// <summary>
    /// 自动
    /// </summary>
    [Description("自动")]
    Auto = 2,
}