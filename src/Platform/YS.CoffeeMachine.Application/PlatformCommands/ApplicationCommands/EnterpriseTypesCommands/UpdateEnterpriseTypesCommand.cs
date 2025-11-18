using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.EnterpriseTypesCommands
{
    public record UpdateEnterpriseTypesCommand(long id, string name, bool astrict) : ICommand<bool>;
}
