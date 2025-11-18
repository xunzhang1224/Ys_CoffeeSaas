using YS.CoffeeMachine.Application.Tools;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.OrderDtos
{
    /// <summary>
    /// 订单信息数据传输对象
    /// </summary>
    public class OrderInfoDto
    {/// <summary>
     /// 订单Id
     /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string DeviceCode { get; set; }

        /// <summary>
        /// 设备型号名称
        /// </summary>
        public string DeviceModelName { get; set; }

        /// <summary>
        /// 企业ID
        /// </summary>
        public long? EnterpriseId { get; set; }

        /// <summary>
        /// 企业名称
        /// </summary>
        public string EnterpriseName { get; set; }

        /// <summary>
        /// 饮品名称
        /// </summary>
        public string BeverageName { get; set; }

        /// <summary>
        /// 消费金额(元)
        /// </summary>
        public decimal Amount { get; set; } = 0;

        /// <summary>
        /// 货币代码
        /// </summary>
        public string CurrencyCode { get; set; } = "CNY";

        /// <summary>
        /// 货币符号
        /// </summary>
        public string CurrencySymbol { get; set; }

        /// <summary>
        /// 付款方案
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        public long PayTime { get; set; }

        /// <summary>
        /// 支付时间Utc
        /// </summary>
        public string PayTimeStr { get { return DateTimeOffset.FromUnixTimeMilliseconds(PayTime).DateTime.ToString("yyyy-MM-dd HH:mm:ss"); } }

        /// <summary>
        /// 支付时间字符串格式
        /// </summary>
        public string PayTimeSpStr => PayTime.ToString("G");

        /// <summary>
        /// 出货结果
        /// </summary>
        public OrderShipmentResult ShipmentResult { get; set; }

        /// <summary>
        /// 出货结果字符串格式
        /// </summary>
        public string ShipmentResultStr => ShipmentResult.GetDescriptionOrValue();

        /// <summary>
        /// 支付结果
        /// </summary>
        public OrderSaleResult SaleResult { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public OrderTypeEnum? OrderType { get; set; }

        /// <summary>
        /// 支付结果字符串格式
        /// </summary>
        public string SaleResultStr => SaleResult.GetDescriptionOrValue();

        /// <summary>
        /// 订单时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 订单时间字符串格式
        /// </summary>
        public string CreateTimeStr => CreateTime.ToString("yyyy-MM-dd HH:mm:ss");

        /// <summary>
        /// 商户单号
        /// </summary>
        public string? ThirdOrderNo { get; set; } = null;

        /// <summary>
        /// 内部订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal ReturnAmount { get; set; } = 0;

        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; } = 1;

        /// <summary>
        /// 线上支付状态
        /// </summary>
        public OrderStatusEnum? OrderStatus { get; set; }
    }
}