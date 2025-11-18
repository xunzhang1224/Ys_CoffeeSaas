using YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.DomesticPaymentCommands
{
    /// <summary>
    /// 微信进件命令
    /// </summary>
    /// <param name="input"></param>
    public record WechatApplymentCommand(M_PaymentWechatApplymentsDto input) : ICommand<bool>;
}