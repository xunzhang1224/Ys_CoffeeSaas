using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.EnterpriseTypesCommands
{
    public record CreateEnterpriseTypesCommand(string name, bool astrict) : ICommand<bool>;
}
