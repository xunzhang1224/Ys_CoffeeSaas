using YS.CoffeeMachine.Application.Tools;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.OrderDtos.H5OrderDtos
{
    /// <summary>
    /// 订单明细
    /// </summary>
    public class H5OrderDetailsDto
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 商户单号
        /// </summary>
        public string? ThirdOrderNo { get; set; } = null;

        /// <summary>
        /// 企业Id
        /// </summary>
        public long EnterpriseId { get; set; }

        /// <summary>
        /// 企业名称
        /// </summary>
        public string EnterpriseName { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string DeviceCode { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public OrderTypeEnum? OrderType { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public string OrderTypeText => OrderType.HasValue ? OrderType.GetDescriptionOrValue() : string.Empty;

        /// <summary>
        /// 付款方案
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// 支付结果
        /// </summary>
        public OrderSaleResult? SaleResult { get; set; }

        /// <summary>
        /// 支付结果字符串格式
        /// </summary>
        public string? SaleResultStr => SaleResult == null ? null : SaleResult.GetDescriptionOrValue();

        /// <summary>
        /// 订单时间字符串格式
        /// </summary>
        public string CreateTimeStr { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        public long PayTime { get; set; }

        /// <summary>
        /// 支付时间Utc
        /// </summary>
        public string PayTimeStr { get { return DateTimeOffset.FromUnixTimeMilliseconds(PayTime).DateTime.ToString("yyyy-MM-dd HH:mm:ss"); } }

        /// <summary>
        /// 实付金额
        /// </summary>
        public decimal PayAmount { get; set; }

        /// <summary>
        /// 货币代码
        /// </summary>
        public string CurrencyCode { get; set; } = "CNY";

        /// <summary>
        /// 货币符号
        /// </summary>
        public string CurrencySymbol { get; set; }

        /// <summary>
        /// 商品信息
        /// </summary>
        public List<OrderProducts> Products { get; set; } = new List<OrderProducts>();
    }
}