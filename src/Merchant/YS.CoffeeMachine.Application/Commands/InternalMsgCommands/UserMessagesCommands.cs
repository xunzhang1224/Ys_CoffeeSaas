using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.InternalMsgCommands
{
    public record MarkAsReadCommands(long Id) : ICommand;

    public record MarkAsPopupShownCommands(long Id) : ICommand;
}