using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.DeviceInfoCommands
{
    /// <summary>
    /// 平台激活设备
    /// </summary>
    /// <param name="deviceBaseInfoId">平台设备id</param>
    /// <param name="enterpriseinfoId">企业id</param>
    /// <param name="deviceName">设备名称</param>
    public record ActivationDeviceCommand(long deviceBaseInfoId, long enterpriseinfoId, string deviceName) : ICommand;
}
