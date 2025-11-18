using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.CurrencyCommands
{
    /// <summary>
    /// 创建币种
    /// </summary>
    /// <param name="code">货币代码</param>
    /// <param name="name">货币名称</param>
    /// <param name="currencySymbol">货币符号</param>
    /// <param name="currencyShowFormat">默认显示格式</param>
    /// <param name="accuracy">金额精度</param>
    /// <param name="roundingType">舍入类型</param>
    /// <param name="enabled">是否开启</param>
    public record CreateCurrencyCommand(string code, string name, string currencySymbol, CurrencyShowFormatEnum currencyShowFormat, int accuracy, RoundingTypeEnum roundingType, EnabledEnum enabled) : ICommand<bool>;
}
