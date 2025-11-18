using YS.Pay.SDK.Response;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.PaymentCommands.PaypalStripePayment
{
    /// <summary>
    /// 创建支付配置
    /// </summary>
    /// <param name="p_PaymentConfigId"></param>
    /// <param name="email"></param>
    /// <param name="remark"></param>
    /// <param name="pictureUrl"></param>
    public record CreatePaymentConfigCommand(long p_PaymentConfigId, string email, string remark, string pictureUrl, string extra) : ICommand<MerchantIncomingResponse>;
}