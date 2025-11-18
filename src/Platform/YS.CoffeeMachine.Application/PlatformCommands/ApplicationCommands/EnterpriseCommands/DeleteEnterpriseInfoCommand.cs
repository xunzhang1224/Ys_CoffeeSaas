using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.EnterpriseCommands
{
    public record DeleteEnterpriseInfoCommand(long id) : ICommand<bool>;
}