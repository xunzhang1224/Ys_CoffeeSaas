using YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.DomesticPaymentCommands
{
    /// <summary>
    /// 支付宝商户进件
    /// </summary>
    public record AlipayApplymentCommand(M_PaymentAlipayApplymentsInput input) : ICommand<bool>;
}