using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.BeveragesCommandHandlers.BeverageInfoCommandHandlers
{
    /// <summary>
    /// 应用指定设备所有饮品信息
    /// </summary>
    /// <param name="context"></param>
    public class AppliedAllBevergeInfoCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<AppliedAllBevergeInfoCommand, bool>
    {
        /// <summary>
        /// 应用指定设备所有饮品信息
        /// </summary>
        public async Task<bool> Handle(AppliedAllBevergeInfoCommand request, CancellationToken cancellationToken)
        {
            if (request.targetDeviceIds.Contains(request.deviceId))
                throw ExceptionHelper.AppFriendly(string.Format(L.Text[nameof(ErrorCodeEnum.D0051)], request.deviceId));
            //获取源设备饮品
            var sourceBevergeList = await context.BeverageInfo.Include(i => i.FormulaInfos).ThenInclude(ti => ti.MaterialBox).Where(w => w.DeviceId == request.deviceId).ToListAsync();
            if (sourceBevergeList.Count == 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0052)]);
            //获取所有目标饮品
            var targetBevergeList = await context.BeverageInfo.Where(w => request.targetDeviceIds.Contains(w.DeviceId)).ToListAsync();
            if (targetBevergeList.Count > 0)
            {
                //批量删除目标设备饮品
                targetBevergeList.ForEach(f =>
                {
                    f.IsDelete = true;
                    f.AddCode(string.Empty);
                });
            }
            //根据饮品获取设备料盒信息
            var deviceIds = targetBevergeList.Select(s => s.DeviceId).Distinct().ToList();
            var deviceDicList = await context.DeviceInfo
                                .Include(i => i.SettingInfo)
                                    .ThenInclude(s => s.MaterialBoxs)
                                .Where(w => deviceIds.Contains(w.Id))
                                .Select(device => new
                                {
                                    DeviceId = device.Id,
                                    device.SettingInfo.MaterialBoxs
                                })
                                .ToDictionaryAsync(
                                    x => x.DeviceId,
                                    x => x.MaterialBoxs.ToList()
                                );
            //需要添加的饮品
            var addBeverageInfo = new List<BeverageInfo>();
            foreach (var item in request.targetDeviceIds)
            {
                foreach (var template in sourceBevergeList)
                {
                    var curInfo = new BeverageInfo(item, template.Name, template.BeverageIcon, template.Temperature, template.Remarks, template.ProductionForecast, template.ForecastQuantity, template.DisplayStatus, false);
                    //更新Code
                    curInfo.AddCodeNotId(template.Code);
                    //遍历模板配方
                    foreach (var temp in template.FormulaInfos)
                    {
                        long? materialBoxId = null;
                        if (temp.MaterialBox != null)
                        {
                            //根据配方模板料盒位置，获取当前设备同位置料盒Id
                            var cBox = deviceDicList[item].FirstOrDefault(w => w.Sort == temp.MaterialBox.Sort);
                            //当前料盒Id（可以为null）
                            materialBoxId = cBox?.Id ?? null;
                        }
                        //目标设备饮品添加配方
                        curInfo.AddFormulaInfo(new FormulaInfo(curInfo.Id, materialBoxId, temp.MaterialBoxName, temp.Sort, temp.FormulaType, temp.Specs));
                    }
                    curInfo.AddRangeFormulaInfos(template.FormulaInfos);
                    addBeverageInfo.Add(curInfo);
                }
            }
            if (addBeverageInfo.Count > 0)
                await context.AddRangeAsync(addBeverageInfo);
            var res = await context.SaveChangesAsync();
            return res > 0;
        }
    }
}
