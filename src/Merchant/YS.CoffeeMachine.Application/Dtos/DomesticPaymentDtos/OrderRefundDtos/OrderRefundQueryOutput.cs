using YS.CoffeeMachine.Domain.Shared.Enum;

namespace YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos.OrderRefundDtos
{
    /// <summary>
    /// 订单退款查询结果
    /// </summary>
    public class OrderRefundQueryOutput
    {
        /// <summary>
        /// 退款状态
        /// </summary>
        public RefundStatusEnum RefundStatus { get; set; }

        /// <summary>
        /// 失败时的原因
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 退款成功时间
        /// </summary>
        public DateTime? SuccessTime { get; set; }
    }
}