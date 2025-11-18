using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.InternalMsgCommands
{
    public record UserReadGlobalMessagesCommands(long messageId, long userId) : ICommand;
}