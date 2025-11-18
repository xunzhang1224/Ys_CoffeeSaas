using YS.CoffeeMachine.Application.Dtos.PaymentDtos;
using YS.Pay.SDK.Response;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.PaymentCommands.SEVMCPayment
{
    /// <summary>
    /// SEVMC创建订单命令
    /// </summary>
    /// <param name="Input"></param>
    public record CreateOrderCommand(SEVMC_CreateOrderInput Input) : ICommand<CreateOrderResponse>;

    /// <summary>
    /// 订单退款
    /// </summary>
    public record OrderRefundCommand(string payOrderId, decimal amount, List<long> orderItems, string? reason, string? CustomContent, string machineCode) : ICommand<bool>;
}