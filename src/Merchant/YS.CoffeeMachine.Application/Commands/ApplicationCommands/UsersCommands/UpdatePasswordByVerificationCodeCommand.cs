using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.ApplicationCommands.UsersCommands
{
    public record UpdatePasswordByVerificationCodeCommand : ICommand<bool>;
}
