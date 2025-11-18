using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.OrderDtos.H5OrderDtos
{
    /// <summary>
    /// 订单查询入参
    /// </summary>
    public class H5OrderInfoInput : QueryRequest
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public long? BaseDeviceId { get; set; } = null;

        /// <summary>
        /// 商户单号
        /// </summary>
        public string? OrderNo { get; set; } = null;

        /// <summary>
        /// 订单时间
        /// </summary>
        public List<DateTime> CreateTimes { get; set; }

        /// <summary>
        /// 出货结果
        /// </summary>
        public OrderShipmentResult? ShipmentResult { get; set; } = null;

        /// <summary>
        /// 支付结果
        /// </summary>
        public OrderSaleResult? SaleResult { get; set; } = null;
    }
}