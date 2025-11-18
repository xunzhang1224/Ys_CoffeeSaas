using YS.Cabinet.Payment.WechatPay;
using YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.DomesticPaymentCommands
{
    /// <summary>
    /// 发送手机验证码命令
    /// </summary>
    /// <param name="phone"></param>
    public record MerchantOnboardingSendPhoneCodeCommand(long systemPaymentMethodId, string phone) : ICommand<bool>;

    /// <summary>
    /// 添加商户支付方式命令
    /// </summary>
    /// <param name="systemPaymentMethodId"></param>
    /// <param name="phone"></param>
    /// <param name="code"></param>
    /// <param name="remark"></param>
    /// <param name="MerchantId"></param>
    public record InsertMerchantPaymentMethodCommand(long systemPaymentMethodId, string phone, string code, string remark, string? MerchantId = null) : ICommand<bool>;

    /// <summary>
    /// 更新商户支付方式备注
    /// </summary>
    /// <param name="merchantPaymentMethodId"></param>
    /// <param name="remark"></param>
    public record ModifyMerchantPaymentMethodRemarkCommand(long merchantPaymentMethodId, string remark) : ICommand<bool>;

    /// <summary>
    /// 添加商户入驻命令
    /// </summary>
    /// <param name="IdDocType"></param>
    public record MerchantOnboardingCommand(WxApplymentIdentityCertTypeEnum IdDocType) : ICommand<bool>;

    /// <summary>
    /// 更改二级商户支付方式状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="enabledEnum"></param>
    public record ModifyPaymentMethodStatusCommand(long id, EnabledEnum enabledEnum) : ICommand<bool>;

    /// <summary>
    /// 删除二级商户支付方式
    /// </summary>
    /// <param name="id"></param>
    public record DeletePaymentMethodCommand(long id) : ICommand<bool>;

    /// <summary>
    /// 商户支付方式与设备绑定/解绑操作
    /// </summary>
    /// <param name="paymentMethodId"></param>
    /// <param name="deviceIds"></param>
    /// <param name="isBind"></param>
    /// <param name="SystemPaymentMethodId"></param>
    public record PaymentMethodBindDevicesCommand(long paymentMethodId, List<long> deviceIds, bool isBind, long? SystemPaymentMethodId = null) : ICommand<List<DeviceBindPaymentMethodDto>>;
}