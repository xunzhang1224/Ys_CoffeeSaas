using YS.CoffeeMachine.Application.Dtos.VerificationCodeDtos;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.VerificationCodeCommands
{
    public record SendVerificationCodeCommand : ICommand<SendVerificationCodeResponseDto>;
}
