using Yitter.IdGenerator;

namespace YS.CoffeeMachine.API.Extensions.Cap.Dtos.PaymentEventSubscribe
{
    /// <summary>
    /// 订单支付消息
    /// </summary>
    public class PaymentMessageDto
    {
        /// <summary>
        /// 有参构造
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderNo"></param>
        /// <param name="payTime"></param>
        /// <param name="facepay"></param>
        public PaymentMessageDto(string? orderId, string? orderNo = "", DateTime? payTime = null, bool facepay = false)
        {
            OrderId = orderId;
            OrderNo = orderNo;
            PayTime = payTime;
            Facepay = facepay;
            TransId = YitIdHelper.NextId().ToString();
        }

        /// <summary>
        /// 无参构造
        /// </summary>
        public PaymentMessageDto() { }

        /// <summary>
        /// 订单号（Order表的OrderId字段）
        /// </summary>
        public string? OrderId { get; set; }

        /// <summary>
        ///  交易订单号(Order表的OrderNo)
        /// </summary>
        public string? OrderNo { get; set; }

        /// <summary>
        /// 支付时间 需要转换成UTC时间
        /// </summary>
        public DateTime? PayTime { get; set; }

        /// <summary>
        /// 是否为支付宝当面付(刷脸付)/微信付款码支付(刷脸付)
        /// </summary>
        public bool Facepay { get; set; }

        /// <summary>
        /// 事务Id(不能为空，唯一Id，建议使用雪花Id)
        /// </summary>
        public string TransId { get; set; }
    }

}
