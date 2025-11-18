using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Polly;
using Yitter.IdGenerator;
using YS.CoffeeMachine.API.Utils;
using YS.CoffeeMachine.Application.Commands.DevicesCommands;
using YS.CoffeeMachine.Application.Dtos.AdvertisementDtos;
using YS.CoffeeMachine.Domain.AggregatesModel.Advertisements;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.Provider.OSS.Interface.Base;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.DevicesCommandHandlers
{
    /// <summary>
    /// 设备广告
    /// </summary>
    public class DeviceAdCommandHandler
    {
    }

    /// <summary>
    /// 创建设备广告
    /// </summary>
    /// <param name="_context"></param>
    public class CreateDeviceAdCommandHandler(CoffeeMachineDbContext _context, IOSSService _oSSService) : ICommandHandler<CreateDeviceAdCommand>
    {
        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Handle(CreateDeviceAdCommand request, CancellationToken cancellationToken)
        {
            var fileOpt = new FileOptionUtils(_context, _oSSService);
            var deviceAdList = new List<DeviceAdvertisement>();
            var deviceAdFileList = new List<DeviceAdvertisementFile>();

            #region 半屏广告

            long halfScreenId = YitIdHelper.NextId();
            var halfScreen = request.deviceAd.HalfScreen;
            var halfInfo = new DeviceAdvertisement(request.deviceAd.DeviceId ?? 0, AdvertisementEnum.HalfScreen, halfScreen.powerOnAdsPlayTime, null, null, halfScreenId);
            deviceAdList.Add(halfInfo);

            foreach (var item in halfScreen.adList)
            {
                var adfId = YitIdHelper.NextId();
                long fileId = 0;
                string newFilePath = string.Empty;
                if (item.file != null)
                {
                    // 自己提交的文件处理方式
                    var fileDic = await fileOpt.FileMove(item.file, item.file.FilePath, 5, adfId);
                    fileId = fileDic.Keys.First();
                    newFilePath = fileDic[fileId];
                }
                else
                {
                    // 如果是用的资源库的图片
                    fileId = await _context.FileManage.AsQueryable().Where(w => w.FilePath == item.Url).Select(s => s.Id).FirstOrDefaultAsync();
                    await fileOpt.CreateFileRelation(fileId, adfId, 5);
                }

                newFilePath = string.IsNullOrWhiteSpace(newFilePath) ? item.Url : newFilePath;

                var ad = new DeviceAdvertisementFile(halfScreenId, newFilePath, item.Name, item.Duration, item.Sort, item.Suffix, false, item.FileLength, item.Enable, adfId);
                deviceAdFileList.Add(ad);
            }

            #endregion

            #region 添加全屏广告

            long fullScreenId = YitIdHelper.NextId();
            var fullScreen = request.deviceAd.FullScreen;
            var fullInfo = new DeviceAdvertisement(request.deviceAd.DeviceId ?? 0, AdvertisementEnum.FullScreen, fullScreen.StandbyAdsPlayTime, fullScreen.StandbyAdsAwaitTime, fullScreen.StandbyAdStatus, fullScreenId);
            deviceAdList.Add(fullInfo);

            foreach (var item in fullScreen.adList)
            {
                var adfId = YitIdHelper.NextId();
                long fileId = 0;
                string newFilePath = string.Empty;
                if (item.file != null)
                {
                    // 自己提交的文件处理方式
                    var fileDic = await fileOpt.FileMove(item.file, item.file.FilePath, 5, adfId);
                    fileId = fileDic.Keys.First();
                    newFilePath = fileDic[fileId];
                }
                else
                {
                    // 如果是用的资源库的图片
                    fileId = await _context.FileManage.AsQueryable().Where(w => w.FilePath == item.Url).Select(s => s.Id).FirstOrDefaultAsync();
                    await fileOpt.CreateFileRelation(fileId, adfId, 5);
                }

                newFilePath = string.IsNullOrWhiteSpace(newFilePath) ? item.Url : newFilePath;

                var ad = new DeviceAdvertisementFile(fullScreenId, newFilePath, item.Name, item.Duration, item.Sort, item.Suffix, true, item.FileLength, item.Enable, adfId);
                deviceAdFileList.Add(ad);
            }

            #endregion

            await _context.AddRangeAsync(deviceAdList);
            await _context.AddRangeAsync(deviceAdFileList);

        }
    }

    /// <summary>
    /// 修改设备广告
    /// </summary>
    /// <param name="_context"></param>
    public class UpdateDeviceAdCommandHandler(CoffeeMachineDbContext _context, IOSSService _oSSService) : ICommandHandler<UpdateDeviceAdCommand>
    {
        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Handle(UpdateDeviceAdCommand request, CancellationToken cancellationToken)
        {
            var fileOpt = new FileOptionUtils(_context, _oSSService);
            var deviceAdFileList = new List<DeviceAdvertisementFile>();

            var deviceAd = await _context.DeviceAdvertisement.AsQueryable().Where(w => w.DeviceId == request.deviceAd.DeviceId).ToListAsync();
            var halfScreen = deviceAd.FirstOrDefault(f => f.Type == AdvertisementEnum.HalfScreen);
            var fullScreen = deviceAd.FirstOrDefault(s => s.Type == AdvertisementEnum.FullScreen);

            var newHalfScreen = request.deviceAd.HalfScreen;
            halfScreen.Update(newHalfScreen.powerOnAdsPlayTime);

            var newFullScreen = request.deviceAd.FullScreen;
            fullScreen.Update(newFullScreen.StandbyAdsPlayTime, newFullScreen.StandbyAdsAwaitTime, newFullScreen.StandbyAdStatus);

            // 修改设备广告基本设置
            _context.UpdateRange(deviceAd);

            // 删除设备广告文件 .Select(s => s.Id)
            var dafInfos = await _context.DeviceAdvertisementFile.AsQueryable().Where(w => w.DeviceAdvertisementId == halfScreen.Id || w.DeviceAdvertisementId == fullScreen.Id).ToListAsync();
            var dafIds = dafInfos.Select(s => s.Id).ToList();
            _context.FileRelation.RemoveRange(await _context.FileRelation.Where(w => dafIds.Contains(w.TargetId)).ToListAsync());

            // 删除设备广告绑定资源
            _context.DeviceAdvertisementFile.RemoveRange(dafInfos);

            // 添加设备广告文件
            foreach (var item in newHalfScreen.adList)
            {
                var adfId = YitIdHelper.NextId();
                long fileId = 0;
                string newFilePath = string.Empty;
                if (item.file != null)
                {
                    // 自己提交的文件处理方式
                    var fileDic = await fileOpt.FileMove(item.file, item.file.FilePath, 5, adfId);
                    fileId = fileDic.Keys.First();
                    newFilePath = fileDic[fileId];
                }
                else
                {
                    // 如果是用的资源库的图片
                    fileId = await _context.FileManage.AsQueryable().Where(w => w.FilePath == item.Url).Select(s => s.Id).FirstOrDefaultAsync();
                    await fileOpt.CreateFileRelation(fileId, adfId, 5);
                }
                newFilePath = string.IsNullOrWhiteSpace(newFilePath) ? item.Url : newFilePath;

                var ad = new DeviceAdvertisementFile(halfScreen.Id, newFilePath, item.Name, item.Duration, item.Sort, item.Suffix, false, item.FileLength, item.Enable, adfId);
                deviceAdFileList.Add(ad);
            }

            foreach (var item in newFullScreen.adList)
            {
                var adfId = YitIdHelper.NextId();
                long fileId = 0;
                string newFilePath = string.Empty;
                if (item.file != null)
                {
                    // 自己提交的文件处理方式
                    var fileDic = await fileOpt.FileMove(item.file, item.file.FilePath, 5, adfId);
                    fileId = fileDic.Keys.First();
                    newFilePath = fileDic[fileId];
                }
                else
                {
                    // 如果是用的资源库的图片
                    fileId = await _context.FileManage.AsQueryable().Where(w => w.FilePath == item.Url).Select(s => s.Id).FirstOrDefaultAsync();
                    await fileOpt.CreateFileRelation(fileId, adfId, 5);
                }

                newFilePath = string.IsNullOrWhiteSpace(newFilePath) ? item.Url : newFilePath;

                var ad = new DeviceAdvertisementFile(fullScreen.Id, newFilePath, item.Name, item.Duration, item.Sort, item.Suffix, true, item.FileLength, item.Enable, adfId);
                deviceAdFileList.Add(ad);
            }

            if (deviceAdFileList.Count > 0)
            {
                await _context.AddRangeAsync(deviceAdFileList);
            }
        }
    }
}
