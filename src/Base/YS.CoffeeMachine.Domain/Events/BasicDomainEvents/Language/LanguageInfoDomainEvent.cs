using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Language;
using YSCore.Base.DomainEvent;

namespace YS.CoffeeMachine.Domain.Events.BasicDomainEvents.Language
{
    public record SetDefaultLanguageDomainEvent(LanguageInfo lang) : IDomainEvent;
    public record CreateLanguageDomainEvent(LanguageInfo lang) : IDomainEvent;
    public record UpdateLanguageDomainEvent(LanguageInfo lang) : IDomainEvent;
    public record DeleteLanguageDomainEvent(LanguageInfo lang) : IDomainEvent;
}
