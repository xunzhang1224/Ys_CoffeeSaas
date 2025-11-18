using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.BeveragesCommandHandlers.BeverageInfoCommandHandlers
{
    /// <summary>
    /// 饮品应用到其他饮品
    /// </summary>
    /// <param name="context"></param>
    public class B_AppliedBeverageInfoCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<B_AppliedBeverageInfoCommand, bool>
    {
        /// <summary>
        /// 饮品应用到其他饮品
        /// </summary>
        public async Task<bool> Handle(B_AppliedBeverageInfoCommand request, CancellationToken cancellationToken)
        {
            //获取所有目标饮品Id
            var beverageIds = request.appliedBeverageInputs.SelectMany(s => s.beverageIds).Select(s => s).ToList();
            //不能应用到自己
            if (beverageIds.Contains(request.beverageId))
                throw ExceptionHelper.AppFriendly(string.Format(L.Text[nameof(ErrorCodeEnum.D0051)], request.beverageId));
            //获取模板饮品（原）
            var template = await context.BeverageInfo.Include(i => i.FormulaInfos).ThenInclude(ti => ti.MaterialBox).FirstAsync(w => w.Id == request.beverageId);
            if (template == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0053)]);
            //获取所有目标饮品
            var targetBeverageList = await context.BeverageInfo.Include(i => i.FormulaInfos).Where(w => beverageIds.Contains(w.Id)).ToListAsync();
            //根据饮品获取设备料盒信息
            var deviceIds = request.appliedBeverageInputs.Select(s => s.deviceId).Distinct().ToList();
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
            foreach (var item in request.appliedBeverageInputs)
            {
                //添加饮品
                if (item.appliedType == BeverageAppliedType.Add)
                {
                    var curInfo = new BeverageInfo(item.deviceId, template.Name, template.BeverageIcon, template.Temperature, template.Remarks, template.ProductionForecast, template.ForecastQuantity, template.DisplayStatus, false);
                    curInfo.AddCodeNotId(template.Code);
                    curInfo.UpdateCodeIsShow(template.CodeIsShow);
                    //遍历模板配方
                    foreach (var temp in template.FormulaInfos)
                    {
                        long? materialBoxId = null;
                        if (temp.MaterialBox != null)
                        {
                            //根据配方模板料盒位置，获取当前设备同位置料盒Id
                            var cBox = deviceDicList[item.deviceId].FirstOrDefault(w => w.Sort == temp.MaterialBox.Sort);
                            //当前料盒Id（可以为null）
                            materialBoxId = cBox?.Id ?? null;
                        }
                        //目标设备饮品添加配方
                        curInfo.AddFormulaInfo(new FormulaInfo(curInfo.Id, materialBoxId, temp.MaterialBoxName, temp.Sort, temp.FormulaType, temp.Specs));
                    }
                    addBeverageInfo.Add(curInfo);
                }
                //替换饮品
                else
                {
                    //当前目标饮品
                    foreach (var beverageId in item.beverageIds)
                    {
                        //当前目标饮品
                        var curTargetBeverage = targetBeverageList.First(w => w.Id == beverageId);

                        // 序列化成 String，添加历史记录
                        string jsonData = JsonSerializer.Serialize(curTargetBeverage, new JsonSerializerOptions
                        {
                            ReferenceHandler = ReferenceHandler.Preserve,
                            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // 禁用特殊字符的转义
                            WriteIndented = true // 美化输出（可选）
                        });
                        curTargetBeverage.InsertBeverageVersions(BeverageVersionTypeEnum.Edit, jsonData);

                        //开始替换内容
                        switch (item.contentType)
                        {
                            //替换基本信息
                            case ReplaceContentType.BaseInfo:
                                curTargetBeverage.Update(template.Name, template.BeverageIcon, template.Temperature, template.Remarks, template.ProductionForecast, template.ForecastQuantity, template.DisplayStatus);
                                curTargetBeverage.AddCodeNotId(template.Code);
                                curTargetBeverage.UpdateCodeIsShow(template.CodeIsShow);
                                break;
                            //替换配方信息
                            case ReplaceContentType.FormulaInfo:
                                //遍历模板配方
                                foreach (var temp in template.FormulaInfos)
                                {
                                    long? materialBoxId = null;
                                    if (temp.MaterialBox != null)
                                    {
                                        //根据配方模板料盒位置，获取当前设备同位置料盒Id
                                        var cBox = deviceDicList[item.deviceId].FirstOrDefault(w => w.Sort == temp.MaterialBox.Sort);
                                        //当前料盒Id（可以为null）
                                        materialBoxId = cBox?.Id ?? null;
                                    }
                                    //目标设备饮品添加配方
                                    curTargetBeverage.AddFormulaInfo(new FormulaInfo(curTargetBeverage.Id, materialBoxId, temp.MaterialBoxName, temp.Sort, temp.FormulaType, temp.Specs));
                                }
                                break;
                            //替换全部
                            case ReplaceContentType.ALL:
                                curTargetBeverage.Update(template.Name, template.BeverageIcon, template.Temperature, template.Remarks, template.ProductionForecast, template.ForecastQuantity, template.DisplayStatus);
                                curTargetBeverage.AddCodeNotId(template.Code);
                                curTargetBeverage.UpdateCodeIsShow(template.CodeIsShow);
                                foreach (var temp in template.FormulaInfos)
                                {
                                    long? materialBoxId = null;
                                    if (temp.MaterialBox != null)
                                    {
                                        //根据配方模板料盒位置，获取当前设备同位置料盒Id
                                        var cBox = deviceDicList[item.deviceId].FirstOrDefault(w => w.Sort == temp.MaterialBox.Sort);
                                        //当前料盒Id（可以为null）
                                        materialBoxId = cBox?.Id ?? null;
                                    }
                                    //目标设备饮品添加配方
                                    curTargetBeverage.AddFormulaInfo(new FormulaInfo(curTargetBeverage.Id, materialBoxId, temp.MaterialBoxName, temp.Sort, temp.FormulaType, temp.Specs));
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            if (addBeverageInfo.Count > 0)
            {
                await context.AddRangeAsync(addBeverageInfo);
                foreach (var item in addBeverageInfo)
                    item.AddCode(string.Empty);//新增的饮品编码为Id保证唯一，且前端不会显示
            }

            context.BeverageInfo.UpdateRange(targetBeverageList);
            var res = await context.SaveChangesAsync();
            return res > 0;
        }
        // 深拷贝配方信息
        private List<FormulaInfo> DeepCloneFormulaInfos(List<FormulaInfo> originalFormulaInfos, long boxId)
        {
            return originalFormulaInfos.Select(f => new FormulaInfo(f.BeverageInfoId, boxId, f.MaterialBoxName, f.Sort, f.FormulaType, f.Specs)).ToList();
        }
    }
}
