using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.UsersCommands
{
    public record RefreshCommand(string refreshToken) : ICommand<LoginResponseDto>;
}
