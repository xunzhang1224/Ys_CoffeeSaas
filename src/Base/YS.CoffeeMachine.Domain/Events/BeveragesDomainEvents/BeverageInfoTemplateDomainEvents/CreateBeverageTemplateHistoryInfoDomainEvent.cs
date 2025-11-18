using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.BeverageWarehouse;
using YSCore.Base.DomainEvent;

namespace YS.CoffeeMachine.Domain.Events.BeveragesDomainEvents.BeverageInfoTemplateDomainEvents
{
    public record CreateBeverageTemplateHistoryInfoDomainEvent(BeverageInfoTemplate beverageInfoTemplate) : IDomainEvent;
}
