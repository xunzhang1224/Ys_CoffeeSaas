using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Polly;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceBaseCommands;
using YS.CoffeeMachine.Application.Dtos.DeviceDots;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YS.Util.Core;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.DeviceBaseCommandHandlers
{
    /// <summary>
    /// UpdateDeviceCapacityCfg
    /// </summary>
    /// <param name="_db"></param>
    public class UpdateDeviceCapacityCfgCommandHandler(CoffeeMachineDbContext _db) : ICommandHandler<UpdateDeviceCapacityCfgCommand, bool>
    {
        /// <summary>
        /// UpdateDeviceCapacityCfg
        /// </summary>
        public async Task<bool> Handle(UpdateDeviceCapacityCfgCommand request, CancellationToken cancellationToken)
        {
            var cfgs = await _db.DeviceCapacityCfg.Where(x => x.DeviceBaseId == request.deviceBaseId && request.dic.Keys.Contains((int)x.CapacityId)).ToListAsync();
            cfgs.ForEach(x => x.Update(request.dic[(int)x.CapacityId]));

            // 修改料盒名称维护到物料表
            if (request.dic.ContainsKey((int)CapabilityIdEnum.BoxName))
            {
                // 最新的料盒名
                var lhs = JsonConvert.DeserializeObject<List<LhDto>>(request.dic[(int)CapabilityIdEnum.BoxName]);

                // 已有料盒
                var olds = await _db.DeviceMaterialInfo.Where(x => x.DeviceBaseId == request.deviceBaseId && x.Type == MaterialTypeEnum.Cassette).ToListAsync();
                if (olds != null && olds.Any())
                {
                    for (int i = 0; i < lhs.Count; i++)
                    {
                        bool issugar = lhs[i].IsSugar;
                        var old = olds.FirstOrDefault(x => x.Index == i + 1);
                        if (old == null)
                        {
                            await _db.AddAsync(new DeviceMaterialInfo(request.deviceBaseId, MaterialTypeEnum.Cassette, i, lhs[i].Name, issugar, lhs[i].Capacity));
                        }
                        else
                        {
                            old.UpdateName(lhs[i].Name);

                            old.UpdateIsSugar(issugar);
                            old.UpdateCapacity(lhs[i].Capacity);
                            _db.Update(old);
                        }
                    }
                }
                else
                {
                    for (int i = 1; i < lhs.Count; i++)
                    {
                        await _db.AddAsync(new DeviceMaterialInfo(request.deviceBaseId, MaterialTypeEnum.Cassette, i, lhs[i].Name, lhs[i].IsSugar, lhs[i].Capacity));
                    }
                }
            }

            // 更新饮品加个小数点位数
            if (request.dic.ContainsKey((int)CapabilityIdEnum.Currency))
            {
                // 获取设备信息
                var deviceBaseInfo = await _db.DeviceBaseInfo.FirstOrDefaultAsync(x => x.Id == request.deviceBaseId);
                if (deviceBaseInfo != null)
                {
                    var deviceInfo = await _db.DeviceInfo.Include(i => i.BeverageInfos).FirstOrDefaultAsync(x => x.DeviceBaseId == deviceBaseInfo.Id);
                    if (deviceInfo != null)
                    {
                        // 最新的货币配置
                        var currencyInfo = JsonConvert.DeserializeObject<CurrencyDto>(request.dic[(int)CapabilityIdEnum.Currency]);
                        if (currencyInfo != null)
                        {
                            deviceInfo.BeverageInfos.ForEach(e =>
                                e.UpdatePriceInfo(Util.Core.Util.TruncateDecimal(e.Price ?? 0, currencyInfo.CurrencyDecimalDigits), Util.Core.Util.TruncateDecimal(e.DiscountedPrice ?? 0, currencyInfo.CurrencyDecimalDigits))
                            );
                            _db.Update(deviceInfo);
                        }
                    }
                }
            }
            return true;
        }
    }

    /// <summary>
    /// 货币配置
    /// </summary>
    public class CurrencyDto
    {
        /// <summary>
        /// 货币小数点位数
        /// </summary>
        public int CurrencyDecimalDigits { get; set; }

        /// <summary>
        /// 货币符号展示位置 0:前 1:后
        /// </summary>
        public int CurrencyPosition { get; set; }

        /// <summary>
        /// 货币符号
        /// </summary>
        public string CurrencyCode { get; set; }
    }
}
