
using Autofac.Core;
using FreeRedis;
using MagicOnion.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using YS.CoffeeMachine.Cap.IServices;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Log;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Iot.Api.Dto;
using YS.CoffeeMachine.Iot.Api.Extensions.Cap.Dto;
using YS.CoffeeMachine.Provider;
using YS.CoffeeMachine.Provider.IServices;

namespace YS.CoffeeMachine.Iot.Api.Extensions.BackTask
{
    /// <summary>
    /// 设备下线任务
    /// </summary>
    /// <param name="_logger"></param>
    /// <param name="_platformDbContext"></param>
    /// <param name="_redisService"></param>
    /// <param name="_timeDb"></param>
    public class DeviceOfflineTask(ILogger<DeviceOfflineTask> _logger,CoffeeMachinePlatformDbContext _platformDbContext,IRedisClient _redisService, IPublishService _publish,
        CoffeeMachineTimescaleDBContext _timeDb) : BackgroundService
    {
        /// <summary>
        /// key
        /// </summary>
        private readonly string key = CacheConst.DeviceOnlineKey;

        /// <summary>
        /// ExecuteAsync
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var deviceOnlines = await _redisService.HGetAllAsync<OnlineDto>(key);
                    if (deviceOnlines == null || !deviceOnlines.Any())
                    {
                        var devicebases = await _platformDbContext.DeviceBaseInfo.Where(x => x.IsOnline == true).ToListAsync();

                        await OffAsync(devicebases);
                    }
                    else
                    {
                        var datas = deviceOnlines?.Values.Where(x => x.Status == true && x.OnDate.AddMinutes(IotConst.DeviceHeartbeatIntervalMinutes) < DateTime.UtcNow)?.ToList();

                        if (datas != null && datas.Any())
                        {
                            var mids = datas.Select(x => x.Mid).ToList();
                            var devicebases = await _platformDbContext.DeviceBaseInfo.Where(x => mids.Contains(x.Mid)).ToListAsync();
                            await OffAsync(devicebases);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "设备下线任务异常");
                    await Task.Delay(5000, stoppingToken);
                }
                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }
        }

        private async Task OffAsync(List<DeviceBaseInfo> devicebases)
        {
            var modelids = devicebases.Select(x => x.DeviceModelId).ToList();
            var baseids = devicebases.Select(x => x.Id).ToList();
            var models = await _platformDbContext.DeviceModel.Where(x => modelids.Contains(x.Id)).ToListAsync();
            var devices = await _platformDbContext.DeviceInfo.Where(x => baseids.Contains(x.DeviceBaseId)).ToListAsync();
            Dictionary<string, OnlineDto> offs = new Dictionary<string, OnlineDto>();
            foreach (var item in devicebases)
            {
                Console.WriteLine($"离线任务执行离线的设备：{item.Mid}");
                item.Offline();
                _platformDbContext.Update(item);
                await _platformDbContext.SaveChangesAsync();
                var model = models?.FirstOrDefault(x => x.Id == item.DeviceModelId);
                var device = devices?.FirstOrDefault(x => x.DeviceBaseId == item.Id);
                var onlineLog = new DeviceOnlineLog(item.Mid, device?.Name ?? "", item.Id, model?.Name ?? "", false, device?.EnterpriseinfoId ?? 0);
                //await _publish.SendMessage(CapConst.CreateDeviceOnlineLog, onlineLog);
                await _timeDb.AddAsync(onlineLog);
                await _timeDb.SaveChangesAsync();
                offs.Add(item.Mid, new OnlineDto() { Mid = item.Mid, Status = false, OffDate = DateTime.UtcNow });
            }
            if (offs.Any())
            {
                await _redisService.HMSetAsync(key, offs);
                await _redisService.ExpireAsync(key, TimeSpan.FromMinutes(2));
            }
        }
    }
}
