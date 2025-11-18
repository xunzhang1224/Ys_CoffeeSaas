using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoTemplateCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.BeveragesCommandHandlers.BeverageInfoTemplateCommandHandlers
{
    /// <summary>
    /// 饮品库饮品应用到设备饮品
    /// </summary>
    /// <param name="context"></param>
    public class AppliedBeverageInfoCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<AppliedBeverageInfoCommand, bool>
    {
        /// <summary>
        /// 饮品库饮品应用到设备饮品
        /// </summary>
        public async Task<bool> Handle(AppliedBeverageInfoCommand request, CancellationToken cancellationToken)
        {
            //获取所有目标饮品Id
            var beverageIds = request.appliedBeverageInputs.SelectMany(s => s.beverageIds).Select(s => s).ToList();
            //饮品模型
            var template = await context.BeverageInfoTemplate.Include(i => i.FormulaInfoTemplates).FirstOrDefaultAsync(w => w.Id == request.templateId);
            if (template == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0053)]);
            //获取所有目标设备
            var allDevices = await context.DeviceInfo.Include(i => i.BeverageInfos).Where(w => request.appliedBeverageInputs.Select(s => s.deviceId).ToList().Contains(w.Id)).ToListAsync();
            //获取所有目标饮品
            var targetBeverageList = await context.BeverageInfo.Include(i => i.FormulaInfos).Where(w => beverageIds.Contains(w.Id)).ToListAsync();
            //获取所有设备料盒信息
            var deviceDicList = await context.DeviceInfo
                .Include(i => i.SettingInfo)
                .ThenInclude(ti => ti.MaterialBoxs)
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
                if (item.appliedType == BeverageAppliedType.Add)
                {
                    var hasCode = allDevices.SelectMany(s => s.BeverageInfos).FirstOrDefault(w => w.DeviceId == item.deviceId && w.Code == template.Code && !string.IsNullOrWhiteSpace(w.Code));
                    if (hasCode != null)
                    {
                        throw ExceptionHelper.AppFriendly(string.Format(L.Text[nameof(ErrorCodeEnum.D0054)], template.Code));
                    }
                    var curInfo = new BeverageInfo(item.deviceId, template.Name, template.BeverageIcon, template.Temperature, template.Remarks, template.ProductionForecast, template.ForecastQuantity, template.DisplayStatus, false);
                    curInfo.AddCodeNotId(template.Code);
                    //遍历模板配方
                    foreach (var temp in template.FormulaInfoTemplates)
                    {
                        long? materialBoxId = null;
                        if (temp.MaterialBoxId != null)
                        {
                            //根据配方模板料盒位置，获取当前设备同位置料盒Id
                            var cBox = deviceDicList[item.deviceId].FirstOrDefault(w => w.Sort == temp.MaterialBoxId);
                            //当前料盒Id（可以为null）
                            materialBoxId = cBox?.Id ?? null;
                        }
                        //目标设备饮品添加配方
                        curInfo.AddFormulaInfo(new FormulaInfo(curInfo.Id, materialBoxId, temp.MaterialBoxName, temp.Sort, temp.FormulaType, temp.Specs));
                    }
                    addBeverageInfo.Add(curInfo);
                }
                else
                {
                    //遍历当前目标饮品
                    foreach (var beverageId in beverageIds)
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

                        switch (item.contentType)
                        {
                            case ReplaceContentType.BaseInfo:
                                curTargetBeverage.Update(template.Name, template.BeverageIcon, template.Temperature, template.Remarks, template.ProductionForecast, template.ForecastQuantity, template.DisplayStatus);
                                //更新Code
                                curTargetBeverage.AddCodeNotId(template.Id.ToString());
                                curTargetBeverage.UpdateCodeIsShow(false);
                                break;
                            case ReplaceContentType.FormulaInfo:
                                if (curTargetBeverage.FormulaInfos != null)
                                    curTargetBeverage.FormulaInfos.Clear();
                                //遍历模板配方
                                foreach (var temp in template.FormulaInfoTemplates)
                                {
                                    long? materialBoxId = null;
                                    if (temp.MaterialBoxId != null)
                                    {
                                        //根据配方模板料盒位置，获取当前设备同位置料盒Id
                                        var cBox = deviceDicList[item.deviceId].FirstOrDefault(w => w.Sort == temp.MaterialBoxId);
                                        //当前料盒Id（可以为null）
                                        materialBoxId = cBox?.Id ?? null;
                                    }
                                    //目标设备饮品添加配方
                                    curTargetBeverage.AddFormulaInfo(new FormulaInfo(curTargetBeverage.Id, materialBoxId, temp.MaterialBoxName, temp.Sort, temp.FormulaType, temp.Specs));
                                }
                                break;
                            case ReplaceContentType.ALL:
                                curTargetBeverage.Update(template.Name, template.BeverageIcon, template.Temperature, template.Remarks, template.ProductionForecast, template.ForecastQuantity, template.DisplayStatus);
                                //更新Code
                                curTargetBeverage.AddCodeNotId(template.Id.ToString());
                                curTargetBeverage.UpdateCodeIsShow(false);
                                if (curTargetBeverage.FormulaInfos != null)
                                    curTargetBeverage.FormulaInfos.Clear();
                                //遍历模板配方
                                foreach (var temp in template.FormulaInfoTemplates)
                                {
                                    long? materialBoxId = null;
                                    if (temp.MaterialBoxId != null)
                                    {
                                        //根据配方模板料盒位置，获取当前设备同位置料盒Id
                                        var cBox = deviceDicList[item.deviceId].FirstOrDefault(w => w.Sort == temp.MaterialBoxId);
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
                await context.AddRangeAsync(addBeverageInfo);
            var res = await context.SaveChangesAsync();
            return res > 0;
        }
    }
}
