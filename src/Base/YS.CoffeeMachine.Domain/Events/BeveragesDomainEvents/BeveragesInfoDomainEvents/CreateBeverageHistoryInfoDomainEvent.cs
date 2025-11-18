using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YSCore.Base.DomainEvent;

namespace YS.CoffeeMachine.Domain.Events.BeveragesDomainEvents.BeveragesInfoDomainEvents
{
    public record CreateBeverageHistoryInfoDomainEvent(BeverageInfo beverageInfo) : IDomainEvent;
}
