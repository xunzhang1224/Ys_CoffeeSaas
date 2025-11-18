using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Docment;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Language;
using YSCore.Base.DomainEvent;

namespace YS.CoffeeMachine.Domain.Events.BasicDomainEvents.Language
{
    public record CreateLanguageTextDomainEvent(LanguageTextEntity entitie) : IDomainEvent;
    public record AddRangeLanguageTextDomainEvent(List<LanguageTextEntity> entities) : IDomainEvent;
    public record DeleteLanguageTextDomainEvent(LanguageTextEntity entitie) : IDomainEvent;
    public record DelRangeLanguageTextDomainEvent(List<LanguageTextEntity> entities) : IDomainEvent;
}
