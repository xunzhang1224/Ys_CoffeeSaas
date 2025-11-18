namespace YS.CoffeeMachine.API.Utils.PaymentUtilDtos
{

    /// <summary>
    /// 支付服务提供商Dto
    /// </summary>
    public class PaymentServiceProviderDto
    {
        /// <summary>
        /// SystemPaymentServiceProvider表的Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 系统支付方式Id
        /// </summary>
        public long SystemPaymentMethodId { get; set; }

        /// <summary>
        /// 小程序AppID
        /// </summary>
        public string AppletAppID { get; set; } = null!;
    }
}
