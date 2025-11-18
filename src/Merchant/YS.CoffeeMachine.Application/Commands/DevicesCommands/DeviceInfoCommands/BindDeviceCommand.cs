using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceInfoCommands
{
    /// <summary>
    /// 设备绑定
    /// </summary>
    /// <param name="deviceBaseInfoId"></param>
    public record class BindDeviceCommand(long deviceBaseInfoId, string? deviceName) : ICommand;
}
