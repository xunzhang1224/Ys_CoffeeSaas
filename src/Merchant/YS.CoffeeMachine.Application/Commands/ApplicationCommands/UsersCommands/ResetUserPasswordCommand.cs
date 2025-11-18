using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.ApplicationCommands.UsersCommands
{
    public record ResetUserPasswordCommand(long id) : ICommand<bool>;
}
