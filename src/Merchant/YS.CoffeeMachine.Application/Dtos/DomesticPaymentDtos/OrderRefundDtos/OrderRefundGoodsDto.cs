namespace YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos.OrderRefundDtos
{
    /// <summary>
    /// 订单退款商品信息
    /// </summary>
    public class OrderRefundGoodsDto
    {
        /// <summary>
        /// 饮品Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 商品名称（Product表的Name）
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal RefundAmount { get; set; } = 0;

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price { get; set; } = 0;
    }
}