using YS.CoffeeMachine.Application.Dtos.InternalMsgDtos;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.InternalMsgCommands
{
    public record SystemMessagesCommands(string title, string content, InternalMsgEnum messageType, bool isPopup, byte priority, long? targetUserId, List<long>? targetGroup, DateTime? expireTime) : ICommand;

    public record UpdateSystemMessagesCommands(long id, string title, string content) : ICommand;

    public record CancelSystemMessagesCommands(long id) : ICommand;
}