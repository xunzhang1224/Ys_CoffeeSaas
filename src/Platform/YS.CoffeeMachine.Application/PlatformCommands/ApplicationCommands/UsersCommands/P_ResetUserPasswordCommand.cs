using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.UsersCommands
{
    public record P_ResetUserPasswordCommand(long id) : ICommand<bool>;
}
