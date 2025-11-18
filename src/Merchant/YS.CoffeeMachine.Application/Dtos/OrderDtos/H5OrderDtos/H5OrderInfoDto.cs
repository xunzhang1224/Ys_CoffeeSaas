using YS.CoffeeMachine.Application.Tools;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.OrderDtos.H5OrderDtos
{
    /// <summary>
    /// 订单信息
    /// </summary>
    public class H5OrderInfoDto
    {
        /// <summary>
        /// 订单主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string? DeviceName { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string DeviceCode { get; set; }

        /// <summary>
        /// 商户单号
        /// </summary>
        public string? ThirdOrderNo { get; set; } = null;

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
        /// 订单号
        /// </summary>
        public string? OrderNo { get; set; } = null;

        /// <summary>
        /// 支付结果
        /// </summary>
        public OrderSaleResult SaleResult { get; set; }

        /// <summary>
        /// 支付结果字符串格式
        /// </summary>
        public string SaleResultStr => SaleResult.GetDescriptionOrValue();

        /// <summary>
        /// 订单创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建时间字符串
        /// </summary>
        public string CreateTimeStr => CreateTime.ToString("G");

        /// <summary>
        /// 商品信息
        /// </summary>
        public List<OrderProducts> Products { get; set; } = new List<OrderProducts>();
    }

    /// <summary>
    /// 订单商品信息
    /// </summary>
    public class OrderProducts
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        public string? ProductName { get; set; } = null;

        /// <summary>
        /// 商品图标
        /// </summary>
        public string ProductIcon { get; set; } = null!;

        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price { get; set; } = 0;

        /// <summary>
        /// 商品数量
        /// </summary>
        public int Quantity { get; set; } = 0;

        /// <summary>
        /// 1-出货成功 0-出货失败
        /// </summary>
        public int Result { get; set; }

        /// <summary>
        /// 出货状态描述
        /// </summary>
        public string ResultStr => Result == 1 ? "出货成功" : "出货失败";
    }
}