using YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos.OrderRefundDtos;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.DomesticPaymentCommands
{
    /// <summary>
    /// 订单退款
    /// </summary>
    /// <param name="input"></param>
    public record OrderRefundCommand(OrderRefundInput input) : ICommand<bool>;
}