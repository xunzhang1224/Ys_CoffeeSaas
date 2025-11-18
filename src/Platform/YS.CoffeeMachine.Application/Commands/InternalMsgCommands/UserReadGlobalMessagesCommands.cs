using YS.CoffeeMachine.Application.Dtos.InternalMsgDtos;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.InternalMsgCommands
{
    public record UserReadGlobalMessagesCommands(UserReadGlobalMessagesDto dto) : ICommand;
}