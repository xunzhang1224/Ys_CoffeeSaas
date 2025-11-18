using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YSCore.Base.DomainEvent;

namespace YS.CoffeeMachine.Domain.Events.DeviceDomainEvents
{
    /// <summary>
    /// 批量设置饮品价格与币种
    /// </summary>
    /// <param name="Device"></param>
    /// <param name="isSend"></param>
    public record SetRangeBeveragePricesDomainEvent(DeviceInfo Device, bool isSend = true) : IDomainEvent;
}
