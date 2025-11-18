using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.EnterpriseCommands
{
    public record ResetAdministratorPasswordCommand(long id) : ICommand<bool>;
}
