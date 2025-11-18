using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.SettingsCommands.SettingInfoCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
using YS.CoffeeMachine.Domain.IPlatformRepositories.SettingsIRepositories;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.SettingsCommandHandlers.SettingInfoCommandHandlers
{
    /// <summary>
    /// 添加设置信息
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="context"></param>
    public class CreateSettingInfoCommandHandler(IPSettingInfoRepository repository, CoffeeMachinePlatformDbContext context) : ICommandHandler<CreateSettingInfoCommand, bool>
    {
        /// <summary>
        /// 添加设置信息
        /// </summary>
        public async Task<bool> Handle(CreateSettingInfoCommand request, CancellationToken cancellationToken)
        {
            if (request.materialBoxs == null || request.materialBoxs.Count != 6)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0071)]);
            var deviceInfo = await context.DeviceInfo.Include(i => i.SettingInfo).FirstOrDefaultAsync(w => w.Id == request.deviceInfoId);
            if (deviceInfo == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0008)]);
            if (deviceInfo.SettingInfo != null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0072)]);
            var materialBoxs = new List<MaterialBox>();
            foreach (var item in request.materialBoxs.OrderBy(x => x.Sort))
            {
                materialBoxs.Add(new MaterialBox(0, item.Name, item.Sort, item.IsActive));
            }
            var info = new SettingInfo(request.deviceInfoId, request.isShowEquipmentNumber, request.interfaceStylesId, request.washType, request.regularWashTime, request.washWarning, request.afterSalesPhone, request.expectedUpdateTime, request.screenBrightness, request.deviceSound, request.administratorPwd, request.replenishmentOfficerPwd, request.startTime, request.startWeek, request.endTime, request.endWeek, request.languageName, request.currencyCode, materialBoxs);
            var res = await repository.AddAsync(info);
            return res != null;

            //var diffs = YS.Util.Core.Util.GetDifferentProperties(deviceInfo.SettingInfo, info);
            //deviceInfo.SetSettingInfo(info, diffs);
            //return true;
        }
    }
}
