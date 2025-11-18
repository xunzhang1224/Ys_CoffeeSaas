namespace YS.CoffeeMachine.Application.Dtos.PaymentDtos
{
    /// <summary>
    /// 订单退款实体数据传输对象
    /// </summary>
    public class OrderRefundEntityDto
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public OrderRefundEntityDto()
        {
        }

        /// <summary>
        /// 子订单Id
        /// </summary>
        public long I { get; set; }

        /// <summary>
        /// 商品Sku
        /// </summary>
        public object PS { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public object A { get; set; }
    }
}