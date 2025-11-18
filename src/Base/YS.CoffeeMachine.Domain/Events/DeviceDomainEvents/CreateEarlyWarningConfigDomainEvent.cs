using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSCore.Base.DomainEvent;

namespace YS.CoffeeMachine.Domain.Events.DeviceDomainEvents
{
    public record CreateEarlyWarningConfigDomainEvent(long deviceId) : IDomainEvent;
}
