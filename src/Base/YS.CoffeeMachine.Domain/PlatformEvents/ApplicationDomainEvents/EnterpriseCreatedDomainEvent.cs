using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSCore.Base.DomainEvent;

namespace YS.CoffeeMachine.Domain.PlatformEvents.ApplicationDomainEvents
{
    public record EnterpriseCreatedDomainEvent(long enterpriseId, string account, string nickName, string areaCode, string phone, string emial) : IDomainEvent;
}
