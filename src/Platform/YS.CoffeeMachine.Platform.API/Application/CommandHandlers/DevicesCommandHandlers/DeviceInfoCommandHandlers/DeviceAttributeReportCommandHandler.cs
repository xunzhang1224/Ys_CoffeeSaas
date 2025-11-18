using Microsoft.EntityFrameworkCore;
using System.Reflection;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceInfoCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.DevicesCommandHandlers.DeviceInfoCommandHandlers
{
    /// <summary>
    /// 设备属性上报
    /// </summary>
    /// <param name="coffeeMachineDb"></param>
    public class DeviceAttributeReportCommandHandler(CoffeeMachinePlatformDbContext coffeeMachineDb) : ICommandHandler<DeviceAttributeReportCommand>
    {
        /// <summary>
        /// 设备属性上报
        /// </summary>
        public async Task Handle(DeviceAttributeReportCommand request, CancellationToken cancellationToken)
        {
            var device = await coffeeMachineDb.DeviceInfo.Include(x => x.DeviceModel).Include(y => y.SettingInfo).ThenInclude(y => y.MaterialBoxs).FirstOrDefaultAsync(x => x.Mid == request.mid);

            if (device == null)
            {
                throw ExceptionHelper.AppFriendly(L.Text[string.Format(nameof(ErrorCodeEnum.D0005), request.mid)]);
            }
            PropertyInfo[] deviceInfo = typeof(DeviceInfo).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo[] settingInfo = typeof(SettingInfo).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var item in request.properties)
            {
                #region 特殊处理
                // 风格
                if (item.Key == "InterfaceStyle")
                {
                    var style = await coffeeMachineDb.InterfaceStyles.FirstOrDefaultAsync(x => x.Code == item.Value);
                    if (style == null)
                    {
                        throw ExceptionHelper.AppFriendly(string.Format(L.Text[nameof(ErrorCodeEnum.D0006)], item.Value));
                    }
                    device.SettingInfo.SetStyle(style.Id);
                    continue;
                }
                // 料盒集合
                if (item.Key == "MaterialBoxs")
                {
                    var i = 0;
                    var materialBoxs = item.Value.Split(',');
                    var materialBoxsOld = device.SettingInfo.MaterialBoxs.OrderBy(x => x.Sort).ToList();
                    foreach (var item1 in materialBoxs)
                    {
                        materialBoxsOld[i].Update(item1);
                        i++;
                    }
                    continue;
                }
                #endregion
                foreach (var p in deviceInfo)
                {
                    if (p.Name == item.Key)
                    {
                        // 赋值
                        p.SetValue(device, item.Value);
                        continue;
                    }
                }
                foreach (var p in settingInfo)
                {
                    if (p.Name == item.Key)
                    {
                        // 赋值
                        p.SetValue(device.SettingInfo, item.Value);
                        continue;
                    }
                }
            }
            coffeeMachineDb.DeviceInfo.Update(device);
        }
    }
}
