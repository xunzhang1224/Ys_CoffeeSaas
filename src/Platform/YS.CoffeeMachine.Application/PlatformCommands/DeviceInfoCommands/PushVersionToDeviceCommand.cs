using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.DeviceDots;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.DeviceInfoCommands
{
    /// <summary>
    /// 推送版本到设备
    /// </summary>
    /// <param name="deviceBaseInfoIds">选择的设备ids</param>
    /// <param name="deviceVersionManageId">id</param>
    public record PushVersionToDeviceCommand(List<long> deviceBaseInfoIds, long deviceVersionManageId, int type, string programTypeName) : ICommand<List<PushVersionDto>>;
}
