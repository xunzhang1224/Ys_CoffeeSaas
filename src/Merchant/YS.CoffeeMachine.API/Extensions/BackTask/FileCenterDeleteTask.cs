using Magicodes.ExporterAndImporter.Core.Extension;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using YS.CoffeeMachine.API.Extensions.Cap.Dtos;
using YS.CoffeeMachine.API.Utils;
using YS.CoffeeMachine.Cap.IServices;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.Shared;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;

namespace YS.CoffeeMachine.API.Extensions.BackTask
{
    /// <summary>
    /// 设备预警任务
    /// </summary>
    /// <param name="Logger"></param>
    /// <param name="_platformDbContext"></param>
    /// <param name="_redisService"></param>
    /// <param name="_timeDb"></param>
    public class FileCenterDeleteTask : BackgroundService
    {
        private readonly IServiceScope _serviceScope;
        private readonly ILogger<DeviceWarningTask> _logger;
        private readonly CoffeeMachinePlatformDbContext _db;

        /// <summary>
        ///  a
        /// </summary>
        /// <param name="scopeFactory"></param>
        public FileCenterDeleteTask(IServiceScopeFactory scopeFactory)
        {
            _serviceScope = scopeFactory.CreateScope();
            _logger = _serviceScope.ServiceProvider.GetRequiredService<ILogger<DeviceWarningTask>>();
            _db = _serviceScope.ServiceProvider.GetRequiredService<CoffeeMachinePlatformDbContext>();
        }

        /// <summary>
        /// ExecuteAsync
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var datas = await _db.FileCenter.Where(x => x.CreateTime.AddMonths(3) < DateTime.UtcNow).ToListAsync();
                    if (datas.Count > 0)
                    {
                        foreach (var data in datas)
                        {
                            data.Update(FileStateEnum.Delete);
                            _db.FileCenter.Update(data);
                        }
                        await _db.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "文件中心过期任务异常");
                    await Task.Delay(5000, stoppingToken);
                }
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }
    }
}
