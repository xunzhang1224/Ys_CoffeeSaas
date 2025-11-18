using Microsoft.Extensions.Primitives;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Language;
using YSCore.Base.DomainEvent;

namespace YS.CoffeeMachine.Domain.Events.BasicDomainEvents
{
    public record AddDictionaryDomainEvent(string key, string value) : IDomainEvent;
}
