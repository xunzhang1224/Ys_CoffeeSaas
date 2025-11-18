using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.UsersCommands
{
    public record P_UpdateUserStatusCommand(long id, UserStatusEnum status) : ICommand<bool>;
}
