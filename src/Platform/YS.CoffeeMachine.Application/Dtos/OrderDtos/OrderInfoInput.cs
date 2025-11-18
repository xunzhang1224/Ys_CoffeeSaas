using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.OrderDtos
{
    /// <summary>
    /// 订单信息查询输入参数
    /// </summary>
    public class OrderInfoInput : QueryRequest
    {
        /// <summary>
        /// 订单时间
        /// </summary>
        public List<DateTime> CreateTimes { get; set; }

        /// <summary>
        /// 饮品名称
        /// </summary>
        public string? BeverageName { get; set; } = null;

        /// <summary>
        /// 出货结果
        /// </summary>
        public OrderShipmentResult? ShipmentResult { get; set; } = null;

        /// <summary>
        /// 支付结果
        /// </summary>
        public OrderSaleResult? SaleResult { get; set; } = null;

        /// <summary>
        /// 支付方式
        /// </summary>
        public string? Provider { get; set; } = null;

        /// <summary>
        /// 支付时间
        /// </summary>
        public List<DateTime>? PayTimes { get; set; } = null;

        /// <summary>
        /// 订单号
        /// </summary>
        public string? OrderNo { get; set; } = null;

        /// <summary>
        /// 商户单号
        /// </summary>
        public string? ThirdOrderNo { get; set; } = null;

        /// <summary>
        /// 企业名称
        /// </summary>
        public string? EnterpriseName { get; set; } = null;

        /// <summary>
        /// 企业Id
        /// </summary>
        public long? EnterpriseId { get; set; } = null;

        /// <summary>
        /// 设备名称
        /// </summary>
        public string? DeviceName { get; set; } = null;

        /// <summary>
        /// 设备编号
        /// </summary>
        public string? DeviceCode { get; set; } = null;

        /// <summary>
        /// 设备型号
        /// </summary>
        public long? DeviceModelId { get; set; } = null;
    }
}