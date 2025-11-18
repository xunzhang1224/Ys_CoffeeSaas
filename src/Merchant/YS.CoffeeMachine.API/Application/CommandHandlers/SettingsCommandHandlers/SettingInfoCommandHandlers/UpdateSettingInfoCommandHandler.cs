using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.API.Utils;
using YS.CoffeeMachine.Application.Commands.SettingsCommands.SettingInfoCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.SettingsCommandHandlers.SettingInfoCommandHandlers
{
    /// <summary>
    /// 编辑设置信息
    /// </summary>
    /// <param name="context"></param>
    public class UpdateSettingInfoCommandHandler(CoffeeMachineDbContext context) : ICommandHandler<UpdateSettingInfoCommand, bool>
    {
        /// <summary>
        /// 编辑设置信息
        /// </summary>
        public async Task<bool> Handle(UpdateSettingInfoCommand request, CancellationToken cancellationToken)
        {
            if (request.materialBoxs == null || request.materialBoxs.Count != 6)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0071)]);
            //var info = await repository.GetSettingInfoByIdAsync(request.id);
            var info = await context.DeviceInfo.Include(x => x.SettingInfo).ThenInclude(y => y.MaterialBoxs).Where(z => z.Id == request.deviceInfoId).FirstAsync();
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

            foreach (var item in info.SettingInfo.MaterialBoxs)
            {
                var newBox = request.materialBoxs.FirstOrDefault(x => x.Sort == item.Sort);
                item.Update(newBox.Name, newBox.IsActive);
            }
            var n = new SettingInfo(request.deviceInfoId, request.isShowEquipmentNumber, request.interfaceStylesId, request.washType,
                request.regularWashTime, request.washWarning, request.afterSalesPhone, request.expectedUpdateTime, request.screenBrightness,
                request.deviceSound, request.administratorPwd, request.replenishmentOfficerPwd,
                request.startTime, request.startWeek, request.endTime, request.endWeek, request.languageName, request.currencyCode, info.SettingInfo.MaterialBoxs);
            var diffs = BasicUtils.GetDifferentProperties(info.SettingInfo, n);
            if (diffs.Keys.Contains("DeviceInfo"))
                diffs.Remove("DeviceInfo");
            if (diffs.Keys.Contains("MaterialBoxs"))
                diffs.Remove("MaterialBoxs");
            if (diffs.Keys.Contains("InterfaceStylesId"))
            {
                var style = await context.InterfaceStyles.Where(z => z.Id == request.interfaceStylesId).FirstAsync();
                diffs.Remove("InterfaceStylesId");
                diffs.Add("InterfaceStyle", style.Code);
            }
            //if (diffs.Keys.Contains("StartWeek"))
            //{
            //    var weeks = YS.Util.Core.Util.GetMatchedEnums<WeekEnum>(request.startWeek);
            //    diffs.Remove("StartWeek");
            //    diffs.Add("StartWeek", JsonConvert.SerializeObject(weeks));
            //}
            //if (diffs.Keys.Contains("EndWeek"))
            //{
            //    var weeks = YS.Util.Core.Util.GetMatchedEnums<WeekEnum>(request.endWeek);
            //    diffs.Remove("EndWeek");
            //    diffs.Add("EndWeek", JsonConvert.SerializeObject(weeks));
            //}
            if (diffs.Count > 0)
            {
                info.SettingInfo.Update(request.deviceInfoId, request.isShowEquipmentNumber,
                    request.interfaceStylesId, request.washType, request.regularWashTime, request.washWarning,
                    request.afterSalesPhone, request.expectedUpdateTime, request.screenBrightness, request.deviceSound,
                    request.administratorPwd, request.replenishmentOfficerPwd, request.startTime, request.startWeek,
                    request.endTime, request.endWeek, request.languageName, request.currencyCode);
                info.SetSettingInfo(diffs);
            }
            return true;
        }
    }
}
