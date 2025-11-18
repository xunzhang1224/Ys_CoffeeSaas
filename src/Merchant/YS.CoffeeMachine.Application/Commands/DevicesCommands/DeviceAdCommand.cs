using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.AdvertisementDtos;
using YS.CoffeeMachine.Application.Dtos.Files;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.DevicesCommands
{
    public record DeviceAdCommand
    {
    }

    /// <summary>
    /// 创建设备广告
    /// </summary>
    public record CreateDeviceAdCommand(DeviceAdInput deviceAd) : ICommand;

    /// <summary>
    /// 修改设备广告
    /// </summary>
    public record UpdateDeviceAdCommand(DeviceAdInput deviceAd) : ICommand;
}
