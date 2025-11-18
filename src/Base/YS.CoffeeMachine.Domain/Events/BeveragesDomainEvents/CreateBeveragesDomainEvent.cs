using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YSCore.Base.DomainEvent;

namespace YS.CoffeeMachine.Domain.Events.BeveragesDomainEvents
{
    /// <summary>
    /// 应用饮品到设备
    /// </summary>
    /// <param name="Device"></param>
    public record CreateBeveragesDomainEvent(DeviceInfo Device, bool isSend = true) : IDomainEvent;
}
