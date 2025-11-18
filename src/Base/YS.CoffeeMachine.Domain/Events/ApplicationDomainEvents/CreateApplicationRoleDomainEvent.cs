using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YSCore.Base.DomainEvent;

namespace YS.CoffeeMachine.Domain.Events.ApplicationDomainEvents
{
    public record CreateApplicationRoleDomainEvent(EnterpriseInfo EnterpriseInfo, long roleId) : IDomainEvent;
}
