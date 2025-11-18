using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageCollectionCommands;
using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.BeveragesCommandHandlers.BeverageCollectionCommandHandlers
{
    /// <summary>
    /// 饮品合集应用到设备
    /// </summary>
    /// <param name="context"></param>
    public class AppliedBeverageCollectionCommandHandler(CoffeeMachinePlatformDbContext context, IMapper mapper) : ICommandHandler<AppliedBeverageCollectionCommand, bool>
    {
        /// <summary>
        /// 饮品合集应用到设备
        /// </summary>
        public async Task<bool> Handle(AppliedBeverageCollectionCommand request, CancellationToken cancellationToken)
        {
            //获取所有目标设备
            var deviceInfos = await context.DeviceInfo
                              .Include(i => i.BeverageInfos)
                              .Include(i => i.SettingInfo)
                              .ThenInclude(s => s.MaterialBoxs)
                              .Where(w => request.beverageCollectionInput.deviceIds.Contains(w.Id)).ToListAsync();
            if (deviceInfos.Count == 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0008)]);
            //获取饮品合集
            var beverageCollection = await context.BeverageCollection.AsNoTracking().FirstOrDefaultAsync(w => w.Id == request.beverageCollectionInput.beverageCollectionId);
            if (beverageCollection == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0045)]);
            //获取饮品集合中饮品历史Id列表
            var curBeverageVersionIds = beverageCollection.BeverageIds.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).Distinct().ToList();
            if (curBeverageVersionIds == null || curBeverageVersionIds.Count == 0)
                throw ExceptionHelper.AppFriendly(string.Format(L.Text[nameof(ErrorCodeEnum.D0046)], beverageCollection.Name));
            //获取饮品合集内的饮品历史列表
            var beverageVersions = await context.BeverageVersion.Where(w => curBeverageVersionIds.Contains(w.Id)).ToListAsync();
            if (beverageVersions.Count == 0)
                throw ExceptionHelper.AppFriendly(string.Format(L.Text[nameof(ErrorCodeEnum.D0047)], beverageCollection.Name));
            //根据饮品获取设备料盒信息
            var deviceDicList = deviceInfos
                                .Select(device => new
                                {
                                    DeviceId = device.Id,
                                    device.SettingInfo.MaterialBoxs
                                })
                                .ToDictionary(
                                    x => x.DeviceId,
                                    x => x.MaterialBoxs.ToList()
                                );
            foreach (var item in deviceInfos)
            {
                //新增饮品列表
                var needAddBeverageInfo = new List<BeverageInfo>();
                //饮品集合应用到设备时，设备原有的饮品全部清除
                if (item.BeverageInfos != null && item.BeverageInfos.Count > 0)
                {
                    //var allBeverageIds = item.BeverageInfos.Select(s => s.Id).ToList();
                    //删除非饮品集合的历史记录
                    foreach (var bitem in item.BeverageInfos)
                    {
                        bitem.IsDelete = true;
                        bitem.AddCodeNotId(bitem.Id.ToString());
                    }
                    await context.SaveChangesAsync();
                    //item.BeverageInfos.Clear();
                }
                //遍历饮品集合的饮品，依次添加到设备饮品中
                foreach (var versions in beverageVersions)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        PropertyNameCaseInsensitive = true
                    };

                    var curVersionsInfo = JsonSerializer.Deserialize<BeverageInfoDto>(versions.BeverageInfoDataString, options);
                    if (curVersionsInfo == null)
                        throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0048)]);
                    var curBeverageInfo = new BeverageInfo(item.Id, curVersionsInfo.Name, curVersionsInfo.BeverageIcon, curVersionsInfo.temperature, curVersionsInfo.Remarks, curVersionsInfo.ProductionForecast, curVersionsInfo.ForecastQuantity, curVersionsInfo.DisplayStatus, false);
                    curBeverageInfo.AddCodeNotId(curVersionsInfo.Code);
                    foreach (var f_info in curVersionsInfo.FormulaInfos)
                    {
                        long? materialBoxId = null;
                        if (f_info.MaterialBoxId != null)
                        {
                            var curMaterialBoxSort = await context.MaterialBox.FirstAsync(w => w.Id == f_info.MaterialBoxId);
                            //根据配方模板料盒位置，获取当前设备同位置料盒Id
                            var cBox = deviceDicList[item.Id].FirstOrDefault(w => w.Sort == curMaterialBoxSort.Sort);
                            //当前料盒Id（可以为null）
                            materialBoxId = cBox?.Id ?? null;
                        }
                        curBeverageInfo.AddFormulaInfo(new FormulaInfo(curBeverageInfo.Id, materialBoxId, f_info.MaterialBoxName, f_info.Sort, f_info.FormulaType, f_info.Specs));
                    }
                    //新增饮品集合
                    needAddBeverageInfo.Add(curBeverageInfo);
                }
                item.BeverageInfos.AddRange(needAddBeverageInfo);
            }
            return true;
        }
    }
}
