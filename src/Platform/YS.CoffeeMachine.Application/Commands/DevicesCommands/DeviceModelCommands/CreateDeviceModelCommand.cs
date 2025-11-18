using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceModelCommands
{
    public record CreateDeviceModelCommand(string key, string name, int maxCassetteCount, string remark, string type) : ICommand<bool>;
}
