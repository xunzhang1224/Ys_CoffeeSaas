namespace YS.CoffeeMachine.Application.Dtos.PaymentDtos
{
    /// <summary>
    /// 订单退款自定义内容输入
    /// </summary>
    public class OrderRefundCustomContentInput
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public OrderRefundCustomContentInput()
        {
        }

        /// <summary>
        /// 订单Id
        /// </summary>
        public object I { get; set; }

        /// <summary>
        /// 操作用户Id
        /// </summary>
        public object C { get; set; }

        /// <summary>
        /// 退款子订单列表
        /// </summary>
        public object Ps { get; set; }
    }
}