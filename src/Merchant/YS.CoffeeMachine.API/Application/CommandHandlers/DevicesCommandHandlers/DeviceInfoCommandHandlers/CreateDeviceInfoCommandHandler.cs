using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceInfoCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
using YS.CoffeeMachine.Domain.IRepositories.DevicesIRepositories;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.DevicesCommandHandlers.DeviceInfoCommandHandlers
{
    /// <summary>
    /// 添加设备
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="context"></param>
    /// <param name="_user"></param>
    public class CreateDeviceInfoCommandHandler(IDeviceInfoRepository repository, CoffeeMachineDbContext context, UserHttpContext _user) : ICommandHandler<CreateDeviceInfoCommand, bool>
    {
        /// <summary>
        /// 添加设备
        /// </summary>
        public async Task<bool> Handle(CreateDeviceInfoCommand request, CancellationToken cancellationToken)
        {
            string countryRegionIds = string.Empty;
            string countryRegionText = string.Empty;
            //if (request.countryRegionIds != null && request.countryRegionIds.Count > 0)
            //{
            //    //获取地区Id集合最后一个
            //    var regionInfo = await repository.GetCountryRegionTextByCountryRegionId(request.countryRegionIds.Last());
            //    if (!regionInfo.Item1)
            //        throw ExceptionHelper.AppFriendly(regionInfo.Item2);
            //    countryRegionText = regionInfo.Item2;//得到详细地区文本
            //    countryRegionIds = String.Join(",", request.countryRegionIds) ?? string.Empty;
            //}
            var info = new DeviceInfo(_user.TenantId, request.name, request.equipmentNumber, request.deviceModelId, request.versionNumber, request.skinPluginVersion,
                request.languagePack, request.updateTime, request.ssid, request.mac, request.iccid, request.usedTrafficThisMonth, request.remainingTrafficThisMonth,
                request.latitude, request.longitude, request.countryId, countryRegionIds, countryRegionText, request.detailedAddress, request.usageScenario, request.posMachineNumber);
            //var mid = request.mid;
            //if (string.IsNullOrWhiteSpace(request.mid))
            //    mid = request.equipmentNumber;
            //info.BindMid(mid);
            var time = DateTime.UtcNow;
            //获取默认风格Id
            var interfaceStyles = await context.InterfaceStyles.FirstOrDefaultAsync();
            if (interfaceStyles == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0058)]);
            //获取当前设备型号
            var deviceModel = await context.DeviceModel.FirstOrDefaultAsync(w => w.Id == request.deviceModelId);
            if (deviceModel == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0059)]);
            //根据设备型号，组装默认料盒
            List<MaterialBox> materialBoxes = new List<MaterialBox>();
            for (int i = 1; i <= deviceModel.MaxCassetteCount; i++)
                materialBoxes.Add(new MaterialBox(0, "", i, true));
            //创建设备默认设置信息
            info.CreateSettingInfo(true, interfaceStyles.Id, WashEnum.Automatic, "00:00:00", null, string.Empty, "00:00:00", 50, 50, "", "", "00:00:00", 0, "00:00:00", 0, string.Empty, string.Empty, materialBoxes);
            //创建设备默认预警信息
            info.CreateDefaultEarlyWarningConfig(info.Id, false, time, false, time, false, time, false, time, false, time, false, 10, false, 0, 0);

            //设备分组
            if (request.groupIds != null)
                info.BindGroups(request.groupIds);

            var res = await repository.AddAsync(info);
            return res != null;
        }
    }
}
