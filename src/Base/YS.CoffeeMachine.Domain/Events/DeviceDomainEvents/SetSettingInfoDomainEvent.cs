using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YSCore.Base.DomainEvent;

namespace YS.CoffeeMachine.Domain.Events.DeviceDomainEvents
{
    public record SetSettingInfoDomainEvent(DeviceInfo Device, Dictionary<string, string> diffs, bool isSend = true) : IDomainEvent;
}
