using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.PaymentDtos
{
    /// <summary>
    /// 创建订单输入参数
    /// </summary>
    public class CreateOrderBaseInput
    {
        /// <summary>
        /// 订单
        /// </summary>
        public long OrderId { get; set; }
        /// <summary>
        /// 设备ID
        /// </summary>
        public long DeviceBaseId { get; set; }

        /// <summary>
        /// 系统订单号（我们自己生成的订单号，微信支付成功后会返回过来）
        /// </summary>
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 设备本地订单编号
        /// </summary>
        public string BizNo { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// 支付时间戳
        /// </summary>
        public long PayTimeSp { get; set; }

        /// <summary>
        /// 企业Id
        /// </summary>
        public long EnterpriseinfoId { get; set; }

        /// <summary>
        /// 货币代码
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// 支付平台商户进件表的唯一标识（Merchant表的Id）
        /// </summary>
        public long MerchantId { get; set; }

        /// <summary>
        /// 实际支付金额（单位:元）
        /// </summary>
        public decimal PayAmount { get; set; }

        /// <summary>
        /// 订单自定义内容(业务平台定义，不能超过200个字)
        /// </summary>
        public string CustomContent { get; set; }

        /// <summary>
        /// 订单详情
        /// </summary>
        public List<OrderDetailDto> OrderDetails { get; set; } = new List<OrderDetailDto>();

        #region 微信支付宝支付相关字段

        /// <summary>
        /// 微信/支付宝支付的商户Id（M_PaymentMethod表的MerchantId）
        /// </summary>
        /// <remarks>
        /// <para>微信支付：特约商户的商户号sub_mchid</para>
        /// <para>支付宝支付：二级商户Id smid</para>
        /// </remarks>
        public string? PaymentMerchantId { get; set; } = null;

        /// <summary>
        /// 系统支付方式表的Id（SystemPaymentMethod表的Id）
        /// </summary>
        public long? SystemPaymentMethodId { get; set; } = null;

        /// <summary>
        /// 支付的服务商表（SystemPaymentServiceProvider表的Id）
        /// </summary>
        public long? SystemPaymentServiceProviderId { get; set; } = null;

        /// <summary>
        /// 微信/支付宝支付方式
        /// </summary>
        public OrderPaymentTypeEnum? OrderPaymentType { get; set; } = null;
        #endregion
    }

    /// <summary>
    /// 订单详情数据传输对象
    /// </summary>
    public class OrderDetailDto
    {
        /// <summary>
        /// 商品id
        /// </summary>
        public long GoodId { get; set; }
        /// <summary>
        /// 货柜编号
        /// </summary>
        public int CounterNo { get; set; } = 0;

        /// <summary>
        /// 货道编号
        /// </summary>
        public int SlotNo { get; set; } = 0;

        /// <summary>
        /// 饮品SKU
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 饮品名称
        /// </summary>
        public string BeverageName { get; set; }

        /// <summary>
        /// 商品单价
        /// </summary>
        public decimal Price { get; set; } = 0;

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; } = 1;
    }
}