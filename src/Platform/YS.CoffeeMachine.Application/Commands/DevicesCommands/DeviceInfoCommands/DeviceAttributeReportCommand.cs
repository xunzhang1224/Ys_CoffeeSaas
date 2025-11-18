using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceInfoCommands
{
    public record DeviceAttributeReportCommand(Dictionary<string,string> telemetrys, Dictionary<string, string> properties,string mid) : ICommand;
}
