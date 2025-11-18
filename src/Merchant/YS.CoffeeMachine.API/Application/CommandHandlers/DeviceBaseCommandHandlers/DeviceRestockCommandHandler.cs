using Aop.Api.Domain;
using Autofac.Core;
using FreeRedis;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceBaseCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.DeviceBaseCommandHandlers
{
    /// <summary>
    /// 远程补货
    /// </summary>
    /// <param name="_db"></param>
    public class DeviceRestockCommandHandler(CoffeeMachineDbContext _db) : ICommandHandler<DeviceRestockCommand, bool>
    {
        /// <summary>
        /// 补货
        /// </summary>
        public async Task<bool> Handle(DeviceRestockCommand request, CancellationToken cancellationToken)
        {
            var olds = await _db.DeviceMaterialInfo.Where(x => request.dic.Keys.Contains(x.Id)).ToListAsync() ?? new List<DeviceMaterialInfo>();
            var device = await _db.DeviceInfo.Where(x => x.DeviceBaseId == request.deviceId).FirstOrDefaultAsync();
            var devicebase = await _db.DeviceBaseInfo.Where(x => x.Id == device.DeviceBaseId).FirstOrDefaultAsync();
            var bhsublogs = new List<DeviceRestockLogSub>();
            foreach (var old in olds)
            {
                var oldvalue = old.Stock;
                var value = request.dic[old.Id].Stock - oldvalue;
                var newvalue = request.dic[old.Id].Stock;
                old.Update(request.dic[old.Id].Capacity, newvalue);
                _db.Update(old);
                if (value != 0)
                    bhsublogs.Add(new DeviceRestockLogSub(HGTypeEnum.CoffeeMachine, old.Id, old.Name, oldvalue, value, newvalue));
            }
            if (bhsublogs.Any())
            {
                var log = new DeviceRestockLog(device?.Id ?? 0, device?.Name ?? "", devicebase?.MachineStickerCode ?? "", device?
                    .DetailedAddress ?? "", RestockTypeEnum.YC, device.EnterpriseinfoId);
                log.AddSubItem(bhsublogs);
                await _db.AddAsync(log);
            }
            return true;
        }
    }
}
