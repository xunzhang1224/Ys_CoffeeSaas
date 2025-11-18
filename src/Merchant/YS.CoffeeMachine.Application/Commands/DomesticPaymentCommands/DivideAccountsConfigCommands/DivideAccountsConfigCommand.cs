using YS.CoffeeMachine.Application.Dtos.DomesticPaymentDtos.DivideAccountsConfigDtos;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.DomesticPaymentCommands.DivideAccountsConfigCommands
{
    /// <summary>
    /// 支付分账配置信息
    /// </summary>
    public record DivideAccountsConfigCommand(DivideAccountsConfigDto input) : ICommand<bool>;

    /// <summary>
    /// 删除分账配置信息
    /// </summary>
    /// <param name="id"></param>
    public record DeleteDivideAccountsConfigCommand(long id) : ICommand<bool>;

    /// <summary>
    /// 更改分账配置状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="enabled"></param>
    public record UpdateDivideAccountsConfigEnabledCommand(long id, EnabledEnum enabled) : ICommand<bool>;

    /// <summary>
    /// 编辑分账配置信息
    /// </summary>
    /// <param name="input"></param>
    public record UpdateDivideAccountsConfigCommand(DeleteDiviDeAccountConfigInput input) : ICommand<bool>;
}