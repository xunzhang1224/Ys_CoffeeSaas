using YS.Pay.SDK.Models;
using YS.Pay.SDK.Response;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.PaymentCommands.SEVMCPayment
{
    /// <summary>
    /// 创建SEVMC支付配置命令
    /// </summary>
    /// <param name="p_PaymentConfigId"></param>
    /// <param name="thirdMerchantId"></param>
    /// <param name="email"></param>
    /// <param name="remark"></param>
    /// <param name="country"></param>
    /// <param name="extra"></param>
    public record CreatePaymentConfigCommand(long p_PaymentConfigId, string thirdMerchantId, string email, string remark, string country, SEVMCMerchantIncomingExtra extra) : ICommand<MerchantIncomingResponse>;
}