using YS.CoffeeMachine.Application.Dtos.PaymentDtos;
using YS.Pay.SDK.Response;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.PaymentCommands.PaypalStripePayment
{
    /// <summary>
    /// Paypal、Stripe创建订单命令
    /// </summary>
    /// <param name="Input"></param>
    public record CreateOrderCommand(CreateOrderBaseInput Input) : ICommand<CreateOrderResponse>;

    /// <summary>
    /// 捕获订单金额
    /// </summary>
    /// <param name="payOrderId"></param>
    public record CaptureOrderCommand(string payOrderId) : ICommand<CaptureOrderResponse>;

    /// <summary>
    /// 订单退款
    /// </summary>
    public record OrderRefundCommand(string payOrderId, decimal amount, List<long> orderItems, string? reason, string? CustomContent) : ICommand<bool>;
}