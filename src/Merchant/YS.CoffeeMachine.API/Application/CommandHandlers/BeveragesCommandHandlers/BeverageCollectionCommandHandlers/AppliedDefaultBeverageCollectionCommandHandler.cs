using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Yitter.IdGenerator;
using YS.CoffeeMachine.API.Utils;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageCollectionCommands;
using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YS.CoffeeMachine.Application.Dtos.Cap;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.BeveragesCommandHandlers.BeverageCollectionCommandHandlers
{
    /// <summary>
    /// 应用默认饮品集合到指定设备
    /// </summary>
    public class AppliedDefaultBeverageCollectionCommandHandler(CoffeeMachineDbContext context) : ICommandHandler<AppliedDefaultBeverageCollectionCommand, CommandDownSends>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CommandDownSends> Handle(AppliedDefaultBeverageCollectionCommand request, CancellationToken cancellationToken)
        {
            var defaultCollectionInfo = await context.P_BeverageCollection.AsNoTracking().FirstOrDefaultAsync(w => w.Id == request.id);
            if (defaultCollectionInfo == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

            var deviceInfo = await context.DeviceInfo.Include(i => i.BeverageInfos).FirstOrDefaultAsync(w => w.Id == request.deviceId);
            if (deviceInfo == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

            var deviceBaseInfo = await context.DeviceBaseInfo.FirstOrDefaultAsync(w => w.Id == deviceInfo.DeviceBaseId);
            if (deviceBaseInfo == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

            List<long> beverageIdList = new List<long>();
            foreach (var id in defaultCollectionInfo.BeverageIds.Split(','))
            {
                if (long.TryParse(id.Trim(), out long result))
                {
                    beverageIdList.Add(result);
                }
            }

            var pBeverageInfos = await context.P_BeverageVersion.AsNoTracking().Where(w => beverageIdList.Contains(w.Id)).ToListAsync();

            var sourceBevergeList = new List<BeverageInfo>();
            if (pBeverageInfos.Any())
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };

                // 新增饮品列表
                var needAddBeverageInfo = new List<BeverageInfo>();
                foreach (var versions in pBeverageInfos)
                {
                    var curVersionsInfo = JsonSerializer.Deserialize<BeverageInfoDto>(versions.BeverageInfoDataString, options);
                    if (curVersionsInfo == null)
                        throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0048)]);

                    var addId = YitIdHelper.NextId();
                    var curBeverageInfo = new BeverageInfo(deviceInfo.Id, curVersionsInfo.Name, curVersionsInfo.BeverageIcon, curVersionsInfo.temperature, curVersionsInfo.Remarks, !string.IsNullOrWhiteSpace(curVersionsInfo.ProductionForecast) ? curVersionsInfo.ProductionForecast : "380", curVersionsInfo.ForecastQuantity,
                        curVersionsInfo.DisplayStatus, false, addId, curVersionsInfo.SellStradgy, curVersionsInfo.Price ?? 0, curVersionsInfo.DiscountedPrice ?? 0);
                    curBeverageInfo.UpdateCodeIsShow(true);

                    foreach (var f_info in curVersionsInfo.FormulaInfos)
                    {
                        var materialBoxId = f_info.FormulaType == FormulaTypeEnum.Lh ? f_info.MaterialBoxId : 1;
                        curBeverageInfo.AddFormulaInfo(new FormulaInfo(curBeverageInfo.Id, materialBoxId, f_info.MaterialBoxName, f_info.Sort, f_info.FormulaType, f_info.Specs));
                    }

                    // 新增饮品集合
                    needAddBeverageInfo.Add(curBeverageInfo);
                }

                sourceBevergeList.AddRange(needAddBeverageInfo);

                // 设备新增默认饮品
                deviceInfo.BeverageInfos.AddRange(needAddBeverageInfo);
                context.Update(deviceInfo);
            }

            // 组装下发饮品信息
            var downSendDto = new CommandDownSends()
            {
                Method = "9026",
                Mid = "饮品合集",
                Mids = new List<string>() { deviceBaseInfo.Mid! },
                Params = JsonSerializer.Serialize(new BasicClassHelper().GetBeverages(sourceBevergeList)),
                IsRecordLog = true
            };
            return downSendDto;

        }
    }
}
