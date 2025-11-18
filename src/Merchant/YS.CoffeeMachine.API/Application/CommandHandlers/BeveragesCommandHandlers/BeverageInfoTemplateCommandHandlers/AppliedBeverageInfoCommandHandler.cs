using Autofac.Core;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Yitter.IdGenerator;
using YS.CoffeeMachine.API.Utils;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoTemplateCommands;
using YS.CoffeeMachine.Application.Dtos.Cap;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Domain.AggregatesModel.BeverageWarehouse;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.AggregatesModel.Files;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.BeveragesCommandHandlers.BeverageInfoTemplateCommandHandlers
{
    /// <summary>
    /// 饮品库饮品应用到设备饮品
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    public class AppliedBeverageInfoCommandHandler(CoffeeMachineDbContext context, IMapper mapper) : ICommandHandler<AppliedBeverageInfoCommand, DrinkCommandDownSends>
    {
        /// <summary>
        /// 饮品库饮品应用到设备饮品
        /// </summary>
        public async Task<DrinkCommandDownSends> Handle(AppliedBeverageInfoCommand request, CancellationToken cancellationToken)
        {
            //获取所有目标饮品Id
            var beverageIds = request.appliedBeverageInputs.SelectMany(s => s.beverageIds).Select(s => s).ToList();

            //饮品模型
            var template = await context.BeverageInfoTemplate.Include(i => i.FormulaInfoTemplates).FirstOrDefaultAsync(w => w.Id == request.templateId);
            if (template == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0053)]);

            //获取所有目标设备
            var allDevices = await context.DeviceInfo.Include(i => i.BeverageInfos).Where(w => request.appliedBeverageInputs.Select(s => s.deviceId).ToList().Contains(w.Id)).ToListAsync();
            //var allBaseDevices = await context.DeviceBaseInfo.Where(w => allDevices.Select(s => s.DeviceBaseId).ToList().Contains(w.Id)).ToListAsync();

            //获取所有目标饮品
            var targetBevergeList = await context.BeverageInfo.Include(i => i.FormulaInfos).Where(w => beverageIds.Contains(w.Id)).ToListAsync();

            //获取所有目标设备信息
            var deviceInfos = await context.DeviceInfo.Where(w => request.appliedBeverageInputs.Select(s => s.deviceId).Contains(w.Id)).ToListAsync();
            var baseDeviceInfos = context.DeviceBaseInfo.Where(w => deviceInfos.Select(s => s.DeviceBaseId).Contains(w.Id)).ToList();

            var downSendDto = new DrinkCommandDownSends()
            {
                Method = "9026",
                Mid = baseDeviceInfos[0].Mid, //string.Format("饮品库:{0}", template.Name),
                Params = JsonSerializer.Serialize(new BasicClassHelper().GetDrinks9026DtoByTemp(new List<BeverageInfoTemplate>() { template }, string.Empty, true)),
                Datas = new Dictionary<string, string?>(),
                Datass = new Dictionary<string, string?>(),
                Yys = new Dictionary<string, Yydto>(),
                IsRecordLog = true
            };

            // 需要添加关系的饮品id（记录新增和替换基本信息的饮品id）
            var beverageIdsForFiles = new List<long>();

            // 需要添加的饮品
            var addBeverageInfo = new List<BeverageInfo>();
            foreach (var item in request.appliedBeverageInputs)
            {
                var trrgetBeverageName = string.Empty;
                var sku = string.Empty;
                var device = deviceInfos.FirstOrDefault(w => w.Id == item.deviceId);
                var baseDeviceInfo = baseDeviceInfos.FirstOrDefault(w => w.Id == device?.DeviceBaseId);

                long newSku = 0;

                if (item.appliedType == BeverageAppliedType.Add)
                {
                    if (template.CodeIsShow)
                    {
                        var hasCode = allDevices.SelectMany(s => s.BeverageInfos).FirstOrDefault(w => w.DeviceId == item.deviceId && w.Code == template.Code);
                        if (hasCode != null)
                        {
                            throw ExceptionHelper.AppFriendly(string.Format(L.Text[nameof(ErrorCodeEnum.D0054)], template.Code));
                        }
                    }

                    var addId = YitIdHelper.NextId();
                    beverageIdsForFiles.Add(addId);
                    var curInfo = new BeverageInfo(item.deviceId, template.Name, template.BeverageIcon, template.Temperature, template.Remarks, template.ProductionForecast, template.ForecastQuantity
                        , template.DisplayStatus, false, addId, template.SellStradgy, productCategoryIds: template.CategoryIds);
                    //curInfo.AddCodeNotId(template.Code);
                    if (template.CodeIsShow)
                    {
                        curInfo.AddCodeNotId(template.Code);
                    }
                    else
                    {
                        curInfo.AddCodeNotId(addId.ToString());
                        newSku = addId;
                    }
                    curInfo.UpdateCodeIsShow(true);
                    //curInfo.UpdateCodeIsShow(template.CodeIsShow);
                    //遍历模板配方
                    foreach (var temp in template.FormulaInfoTemplates)
                    {
                        //long? materialBoxId = null;
                        //if (temp.MaterialBoxId != null)
                        //{
                        //    //根据配方模板料盒位置，获取当前设备同位置料盒Id
                        //    var cBox = deviceDicList[item.deviceId].FirstOrDefault(w => w.Sort == temp.MaterialBoxId);
                        //    //当前料盒Id（可以为null）
                        //    materialBoxId = cBox?.Id ?? null;
                        //}
                        //目标设备饮品添加配方
                        curInfo.AddFormulaInfo(new FormulaInfo(curInfo.Id, temp.MaterialBoxId, temp.MaterialBoxName, temp.Sort, temp.FormulaType, temp.Specs));
                    }
                    addBeverageInfo.Add(curInfo);
                }
                else
                {
                    var curTargetBeverage = targetBevergeList.First(w => w.Id == item.beverageIds[0]);
                    //遍历当前目标饮品
                    //foreach (var beverageId in beverageIds)
                    //{
                    //当前目标饮品
                    //var curTargetBeverage = targetBevergeList.First(w => w.Id == beverageId);
                    trrgetBeverageName = curTargetBeverage.Name;
                    // 序列化成 String，添加历史记录
                    string jsonData = JsonSerializer.Serialize(curTargetBeverage, new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve,
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // 禁用特殊字符的转义
                        WriteIndented = true // 美化输出（可选）
                    });
                    curTargetBeverage.InsertBeverageVersions(BeverageVersionTypeEnum.Edit, jsonData);

                    ////修改信息，记录饮品Id和编码
                    //if (device != null && baseDeviceInfo != null)
                    //    downSendDto.Datass.Add(baseDeviceInfo!.Mid, JsonSerializer.Serialize(new BasicClassHelper().GetDrinks9026Dto(new List<BeverageInfo>() { curTargetBeverage }, baseDeviceInfo.Mid)));

                    switch (item.contentType)
                    {
                        case ReplaceContentType.BaseInfo:
                            //替换部分信息记录SKU
                            sku = curTargetBeverage.Code;
                            beverageIdsForFiles.Add(curTargetBeverage.Id);
                            curTargetBeverage.Update(template.Name, template.BeverageIcon, template.Temperature, template.Remarks, template.ProductionForecast, template.ForecastQuantity, template.DisplayStatus, template.SellStradgy, productCategoryIds: template.CategoryIds);
                            //更新Code
                            curTargetBeverage.AddCodeNotId(template.Code);
                            //更新Code信息
                            //curTargetBeverage.UpdateCodeIsShow(template.CodeIsShow);
                            curTargetBeverage.UpdateCodeIsShow(true);
                            break;
                        case ReplaceContentType.FormulaInfo:
                            if (curTargetBeverage.FormulaInfos != null)
                                curTargetBeverage.FormulaInfos.Clear();
                            //遍历模板配方
                            foreach (var temp in template.FormulaInfoTemplates)
                            {
                                //目标设备饮品添加配方
                                curTargetBeverage.AddFormulaInfo(new FormulaInfo(curTargetBeverage.Id, temp.MaterialBoxId, temp.MaterialBoxName, temp.Sort, temp.FormulaType, temp.Specs));
                            }

                            //替换部分信息记录SKU
                            sku = curTargetBeverage.Code;
                            break;
                        case ReplaceContentType.ALL:

                            //替换部分信息记录SKU
                            sku = curTargetBeverage.Code;

                            beverageIdsForFiles.Add(curTargetBeverage.Id);
                            curTargetBeverage.Update(template.Name, template.BeverageIcon, template.Temperature, template.Remarks, template.ProductionForecast, template.ForecastQuantity, template.DisplayStatus, template.SellStradgy, productCategoryIds: template.CategoryIds);
                            if (template.CodeIsShow)
                                curTargetBeverage.AddCodeNotId(template.Code);
                            //更新Code信息
                            //curTargetBeverage.UpdateCodeIsShow(template.CodeIsShow);
                            curTargetBeverage.UpdateCodeIsShow(true);
                            //更新Code
                            curTargetBeverage.AddCodeNotId(template.Code);
                            if (curTargetBeverage.FormulaInfos != null)
                                curTargetBeverage.FormulaInfos.Clear();
                            //遍历模板配方
                            foreach (var temp in template.FormulaInfoTemplates)
                            {
                                //目标设备饮品添加配方
                                curTargetBeverage.AddFormulaInfo(new FormulaInfo(curTargetBeverage.Id, temp.MaterialBoxId, temp.MaterialBoxName, temp.Sort, temp.FormulaType, temp.Specs));
                            }
                            break;
                        default:
                            break;
                    }
                    //}

                    //修改信息，记录饮品Id和编码
                    if (device != null && baseDeviceInfo != null)
                        downSendDto.Datass.Add(baseDeviceInfo!.Mid, JsonSerializer.Serialize(new BasicClassHelper().GetDrinks9026Dto(new List<BeverageInfo>() { curTargetBeverage }, baseDeviceInfo.Mid)));
                }
                if (baseDeviceInfo != null)
                {
                    downSendDto.Datas.Add(baseDeviceInfo!.Mid, sku);
                    downSendDto.Yys.Add(baseDeviceInfo!.Mid, new Yydto()
                    {
                        AppliedType = item.appliedType,
                        ReplaceTarget = trrgetBeverageName
                    });

                    if (item.appliedType == BeverageAppliedType.Add && newSku != 0)
                    {
                        downSendDto.Yys[baseDeviceInfo!.Mid].NewSku = newSku;
                    }
                }
            }

            #region 绑定文件关系

            var fileManageInfo = await context.FileManage.AsQueryable().Where(w => template.BeverageIcon == w.FilePath).FirstOrDefaultAsync();
            if (fileManageInfo != null && beverageIdsForFiles.Count > 0)
            {
                // 先去掉已有的绑定关联
                context.FileRelation.RemoveRange(context.FileRelation.Where(w => beverageIdsForFiles.Contains(w.TargetId)));

                var fileRelations = new List<FileRelation>();
                foreach (var item in beverageIdsForFiles)
                {
                    var fileRelation = new FileRelation(fileManageInfo.Id, item, 1);
                    fileRelations.Add(fileRelation);
                }

                await context.AddRangeAsync(fileRelations);
            }
            #endregion

            if (addBeverageInfo.Count > 0)
                await context.AddRangeAsync(addBeverageInfo);

            //await context.SaveChangesAsync();
            return downSendDto;
        }
    }
}