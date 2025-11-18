using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using YS.CoffeeMachine.API.Extensions.Cap.Dtos;
using YS.CoffeeMachine.Application.Dtos.DeviceDots;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.OperationLog;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Infrastructure;
using YSCore.Base.Localization;
using YSCore.CoffeeMachine.SignalR.Services;

namespace YS.CoffeeMachine.API.Extensions.Cap.Subscribers
{
    /// <summary>
    /// 指标推送前端
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="signalRService"></param>
    public class MetricNoticeSubscriber(CoffeeMachinePlatformDbContext _db, ISignalRService signalRService) : ICapSubscribe
    {
        /// <summary>
        /// 指标推送前端
        /// </summary>
        [CapSubscribe(CapConst.MetricNotice)]
        public async Task Handle(string input)
        {
            var deviceBase = await _db.DeviceBaseInfo.FirstOrDefaultAsync(x => x.Mid == input);
            if (deviceBase != null)
            {
                var msg = await _db.DeviceMetrics.Where(x => x.DeviceBaseId == deviceBase.Id)
                     .Select(s => new DeviceMetricsOutput
                     {
                         DeviceBaseId = s.DeviceBaseId,
                         CounterNo = s.CounterNo,
                         MetricType = s.MetricType,
                         Index = s.Index,
                         Value = s.Value,
                         Status = s.Status,
                         Description = s.Description,
                         Type = s.Type
                     })
                    .ToListAsync();
                foreach (var item in msg)
                {
                    item.Description = L.Text["MetricType_" + (int)item.MetricType] == "MetricType_" + (int)item.MetricType ? item.Description : L.Text["MetricType_" + (int)item.MetricType];
                    //item.Description = L.Text["MetricType_" + item.MetricType];
                }

                var device = await _db.DeviceInfo.FirstOrDefaultAsync(x => x.DeviceBaseId == deviceBase.Id);
                if (msg != null && device != null)
                    await signalRService.SendMessageToGroupAsync(device.EnterpriseinfoId.ToString(), JsonConvert.SerializeObject(msg), CapConst.MetricNotice);
            }
        }

    }
}
