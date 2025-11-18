using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.DeviceInfoCommands
{
    /// <summary>
    /// 创建管理
    /// </summary>
    public record CreateDeviceVersionManageCommand(string name, string deviceType, string? versionNumber, long? deviceModelId, int programType, int versionType, string url, string remark, string? programTypeName) : ICommand;

    /// <summary>
    /// 更新管理状态
    /// </summary>
    public record UpdateDeviceVersionManageStateCommand(long id, EnabledEnum enabled) : ICommand;
}
