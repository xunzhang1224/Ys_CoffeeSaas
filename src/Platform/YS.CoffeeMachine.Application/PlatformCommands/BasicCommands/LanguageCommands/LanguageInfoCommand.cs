using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.BasicCommands.LanguageCommands
{
    public record CreateLanguageInfoCommand(string name, string code, EnabledEnum isEnabled = EnabledEnum.Enable, IsDefaultEnum isDefault = IsDefaultEnum.No) : ICommand<bool>;
    public record DeleteLanguageInfoCommand(string code) : ICommand<bool>;
    public record UpdateLanguageInfoCommand(string name, string code, EnabledEnum isEnabled, IsDefaultEnum isDefault) : ICommand<bool>;
}
