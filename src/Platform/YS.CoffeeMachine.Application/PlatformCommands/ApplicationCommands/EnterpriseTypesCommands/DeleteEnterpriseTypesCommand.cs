using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.EnterpriseTypesCommands
{
    public record DeleteEnterpriseTypesCommand(long id) : ICommand<bool>;
}
