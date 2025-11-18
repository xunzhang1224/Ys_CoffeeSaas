using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceInfoCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.IRepositories.DevicesIRepositories;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.DevicesCommandHandlers.DeviceInfoCommandHandlers
{
    /// <summary>
    /// 编辑设备
    /// </summary>
    /// <param name="repository"></param>
    public class UpdateDeviceInfoCommandHandler(IDeviceInfoRepository repository, CoffeeMachineDbContext _db) : ICommandHandler<UpdateDeviceInfoCommand, bool>
    {
        /// <summary>
        /// 更新设备
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(UpdateDeviceInfoCommand request, CancellationToken cancellationToken)
        {
            var info = await repository.GetByIdAsync(request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly("设备不存在");
            var regionText = info.CountryRegionText;
            string countryRegionIds = string.Empty;
            //更新地区
            //if (request.countryRegionIds != null && request.countryRegionIds.Count > 0)
            //{
            //    var regionInfo = await repository.GetCountryRegionTextByCountryRegionId(request.countryRegionIds.Last());
            //    if (!regionInfo.Item1)
            //        throw ExceptionHelper.AppFriendly(regionInfo.Item2);
            //    regionText = regionInfo.Item2;
            //    countryRegionIds = string.Join(",", request.countryRegionIds) ?? string.Empty;
            //}
            var device = await _db.DeviceInfo.FirstOrDefaultAsync(x => x.Name == request.name && x.Id != request.id);
            if (device != null)
                throw ExceptionHelper.AppFriendly("设备名称重复"/*L.Text[nameof(ErrorCodeEnum.C0012)]*/);
            info.Update(request.name, request.equipmentNumber, /*info.DeviceModelId,*/ request.versionNumber, request.skinPluginVersion, request.languagePack,
                request.updateTime, request.ssid, request.mac, request.iccid, request.usedTrafficThisMonth, request.remainingTrafficThisMonth, request.latitude,
                request.longitude, request.countryId, countryRegionIds, regionText, request.detailedAddress, request.posMachineNumber);
            //更新设备状态
            if (request.deviceStatus != null)
                info.UpdateStatus(request.deviceStatus.Value);
            if (request.usageScenario != null)
                info.UpdateUsageScenario(request.usageScenario.Value);
            //更新设备最近上线时间
            if (request.latestOnlineTime != null)
                info.UpdateLatestOnlineTime(request.latestOnlineTime.Value);
            //更新设备最近下线状态
            if (request.latestOfflineTime != null)
                info.UpdateLatestOfflineTime(request.latestOfflineTime.Value);
            //设备分组
            if (request.groupIds != null)
                info.BindGroups(request.groupIds);
            var res = repository.UpdateAsync(info);
            return res != null;
        }
    }

    /// <summary>
    /// 修改
    /// </summary>
    public class UpdateDeviceInfoByDicCommandHandler(CoffeeMachineDbContext _db) : ICommandHandler<UpdateDeviceInfoByDicCommand, bool>
    {
        /// <summary>
        /// a1
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(UpdateDeviceInfoByDicCommand request, CancellationToken cancellationToken)
        {
            var info = await _db.DeviceInfo.FirstOrDefaultAsync(x => x.Id == request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(string.Format(L.Text[nameof(ErrorCodeEnum.D0005)], request.id));
            info.UpdateFromDictionary(request.dics);
            _db.Update(info);
            return true;
        }
    }

    /// <summary>
    /// 修改设备名称
    /// </summary>
    public class UpdateDeviceNameCommandHandler(CoffeeMachineDbContext _db) : ICommandHandler<UpdateDeviceNameCommand, bool>
    {
        /// <summary>
        /// a1
        /// </summary>
        public async Task<bool> Handle(UpdateDeviceNameCommand request, CancellationToken cancellationToken)
        {
            var info = await _db.DeviceInfo.FirstOrDefaultAsync(x => x.Id == request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(string.Format(L.Text[nameof(ErrorCodeEnum.D0005)], request.id));
            info.UpdateName(request.name);
            _db.Update(info);
            return true;
        }
    }

    /// <summary>
    /// 修改设备位置
    /// </summary>
    public class UpdateDevicePointCommandHandler(CoffeeMachineDbContext _db) : ICommandHandler<UpdateDevicePointCommand, bool>
    {
        /// <summary>
        /// a1
        /// </summary>
        public async Task<bool> Handle(UpdateDevicePointCommand request, CancellationToken cancellationToken)
        {
            var info = await _db.DeviceInfo.FirstOrDefaultAsync(x => x.Id == request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(string.Format(L.Text[nameof(ErrorCodeEnum.D0005)], request.id));
            info.UpdatePoint(request.province, request.city, request.district, request.street, request.detailedAddress, request.lat, request.lng);
            _db.Update(info);
            return true;
        }
    }
}