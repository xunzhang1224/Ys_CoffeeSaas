using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.Pay.SDK.Response;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.PaymentCommands
{
    public record PaymentCallbackCommand(RequestTopicEnum requestTopic, string content) : ICommand<PublicResponse>;
}
