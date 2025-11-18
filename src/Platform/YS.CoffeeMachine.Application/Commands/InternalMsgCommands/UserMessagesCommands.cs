using YS.CoffeeMachine.Application.Dtos.InternalMsgDtos;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.InternalMsgCommands
{
    public record UserMessagesCommands(UserMessagesDto dto) : ICommand;

    public record MarkAsReadCommands(long Id) : ICommand;

    public record MarkAsPopupShownCommands(long Id) : ICommand;
}