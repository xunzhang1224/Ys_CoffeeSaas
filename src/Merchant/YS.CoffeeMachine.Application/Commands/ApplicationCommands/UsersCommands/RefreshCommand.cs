using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.ApplicationCommands.UsersCommands
{
    public record RefreshCommand(string refreshToken) : ICommand<LoginResponseDto>;
}