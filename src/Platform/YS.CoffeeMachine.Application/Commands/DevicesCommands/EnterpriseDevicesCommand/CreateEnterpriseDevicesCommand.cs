using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.DevicesCommands.EnterpriseDevicesCommand
{
    public record CreateEnterpriseDevicesCommand(List<long> deviceIds, long enterpriseId, DeviceAllocationEnum allocationEnum, DateTime? recyclingTime) : ICommand<bool>;
}
