using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.OrderDtos.H5OrderDtos
{
    /// <summary>
    /// 退款列表查询入参
    /// </summary>
    public class H5OrderRefundDetailListDto
    {
        /// <summary>
        /// 子订单号（OrderDetailId表的Id字段）
        /// </summary>
        public long OrderDetailId { get; set; }

        /// <summary>
        /// 饮品SKU
        /// </summary>
        public string? ItemCode { get; set; } = null;

        /// <summary>
        /// 商品名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 商品主图（Product表的MainImage）
        /// </summary>
        public string? MainImage { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 子订单商品金额
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 子订单商品总额
        /// </summary>
        public decimal Amount { get { return Quantity * Price; } }

        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal RefundAmount { get; set; } = 0;

        /// <summary>
        /// 退款状态
        /// </summary>
        public OrderDetailRerundStatusEnum RefundStatus
        {
            get
            {
                if (Amount == RefundAmount || RefundAmount == -1)
                    return OrderDetailRerundStatusEnum.FullRefund;
                if (Amount > RefundAmount && RefundAmount > 0)
                    return OrderDetailRerundStatusEnum.PartialRefund;
                return OrderDetailRerundStatusEnum.NotRefunded;
            }
        }
    }
}