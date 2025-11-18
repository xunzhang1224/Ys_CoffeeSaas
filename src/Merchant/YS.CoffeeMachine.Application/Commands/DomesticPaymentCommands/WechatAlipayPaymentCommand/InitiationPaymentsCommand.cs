using YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos.WechatAlipayPaymentDtos;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.DomesticPaymentCommands.WechatAlipayPaymentCommand
{
    /// <summary>
    /// 发起微信支付宝Native二维码扫码支付命令
    /// </summary>
    public record CreateOrderCommand(CreateNativeOrderResponse Input) : ICommand<CreateNativeOrderOutput>;

    /// <summary>
    /// 验证商户Id是否存在命令
    /// </summary>
    /// <param name="OrderPaymentType"></param>
    /// <param name="SystemPaymentMethodId"></param>
    /// <param name="MerchantId"></param>
    public record VerifyMerchantIdCommand(OrderPaymentTypeEnum OrderPaymentType, long SystemPaymentMethodId, string MerchantId) : ICommand<bool>;
}