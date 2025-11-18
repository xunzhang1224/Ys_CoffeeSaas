using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.UsersCommands
{
    public record LoginCommand(string account, string password) : ICommand<LoginResponseDto>;
}
