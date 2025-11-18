using Aop.Api.Domain;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.DevicesCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.DevicesCommandHandlers
{
    /// <summary>
    /// 支付方式设置
    /// </summary>
    /// <param name="_db"></param>
    public class UpdateDevicesZFTypeCommandHandler(CoffeeMachineDbContext _db) : ICommandHandler<UpdateDevicesZFTypeCommand, bool>
    {
        /// <summary>
        /// a
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(UpdateDevicesZFTypeCommand request, CancellationToken cancellationToken)
        {
            var bases = await _db.DeviceBaseInfo.Where(x => request.dics.Keys.Contains(x.Mid)).ToListAsync();
            var baseids = bases.Select(x => x.Id);
            var cfs = await _db.DeviceCapacityCfg.Where(x => baseids.Contains(x.DeviceBaseId) && x.CapacityId == Domain.Shared.Enum.CapabilityIdEnum.PayTypes).ToListAsync();
            foreach (var item in request.dics)
            {
                var baseid = bases.FirstOrDefault(x => x.Mid == item.Key)?.Id;
                if (baseid == null)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0013)]);
                if (cfs == null || cfs.FirstOrDefault(x => x.DeviceBaseId == baseid) == null)
                {
                    var cfginfo = new DeviceCapacityCfg(baseid ?? 0, CapabilityIdEnum.PayTypes, "支付方式",
                        CapacityTypeEnum.Software, item.Value, PremissionTypeEnum.R,
                        StructureTypeEnum.JsonArray);
                    await _db.AddAsync(cfginfo);
                }
                else
                {
                    var cfg = cfs.First(x => x.DeviceBaseId == baseid);
                    cfg.Update(item.Value);
                    _db.Update(cfg);
                }
            }
            return true;
        }
    }
}
