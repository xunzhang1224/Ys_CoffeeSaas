using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Yitter.IdGenerator;
using YS.CoffeeMachine.API.Utils;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoCommands;
using YS.CoffeeMachine.Application.Dtos.Cap;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Domain.AggregatesModel.Files;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.BeveragesCommandHandlers.BeverageInfoCommandHandlers
{
    /// <summary>
    /// 饮品应用到其他饮品
    /// </summary>
    /// <param name="context"></param>
    public class B_AppliedBeverageInfoCommandHandler(CoffeeMachineDbContext context) : ICommandHandler<B_AppliedBeverageInfoCommand, DrinkCommandDownSends>
    {
        /// <summary>
        /// 饮品应用到其他饮品
        /// </summary>
        public async Task<DrinkCommandDownSends> Handle(B_AppliedBeverageInfoCommand request, CancellationToken cancellationToken)
        {
            // 获取所有目标饮品Id
            var beverageIds = request.appliedBeverageInputs.SelectMany(s => s.beverageIds).Select(s => s).ToList();

            // 不能应用到自己
            if (beverageIds.Contains(request.beverageId))
                throw ExceptionHelper.AppFriendly(string.Format(L.Text[nameof(ErrorCodeEnum.D0051)], request.beverageId));

            // 获取模板饮品（原）
            var template = await context.BeverageInfo.Include(i => i.FormulaInfos).ThenInclude(ti => ti.MaterialBox).FirstAsync(w => w.Id == request.beverageId);

            if (template == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0053)]);
            // 获取所有目标饮品
            var targetBeverageList = await context.BeverageInfo.Include(i => i.FormulaInfos).Where(w => beverageIds.Contains(w.Id)).ToListAsync();

            // 获取所有目标设备信息
            var deviceInfos = await context.DeviceInfo.Where(w => request.appliedBeverageInputs.Select(s => s.deviceId).Contains(w.Id)).ToListAsync();
            var baseDeviceInfos = context.DeviceBaseInfo.Where(w => deviceInfos.Select(s => s.DeviceBaseId).Contains(w.Id)).ToList();

            var sourceDevice = context.DeviceInfo.FirstOrDefault(w => w.Id == template.DeviceId);
            if (sourceDevice == null)
                throw ExceptionHelper.AppFriendly(string.Format(L.Text[nameof(ErrorCodeEnum.D0005)], template.DeviceId));
            var baseDevice = context.DeviceBaseInfo.FirstOrDefault(w => w.Id == sourceDevice.DeviceBaseId);
            if (baseDevice == null)
                throw ExceptionHelper.AppFriendly(string.Format(L.Text[nameof(ErrorCodeEnum.D0005)], sourceDevice.DeviceBaseId));
            // 饮品应用下发信息
            var downSendDto = new DrinkCommandDownSends()
            {
                Method = "9026",
                Mid = baseDevice!.Mid,
                Params = JsonSerializer.Serialize(new BasicClassHelper().GetDrinks9026Dto(new List<BeverageInfo>() { template }, string.Empty, true)),
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

                // 添加饮品
                if (item.appliedType == BeverageAppliedType.Add)
                {
                    var addId = YitIdHelper.NextId();
                    beverageIdsForFiles.Add(addId);
                    var curInfo = new BeverageInfo(item.deviceId, template.Name, template.BeverageIcon, template.Temperature, template.Remarks, template.ProductionForecast
                        , template.ForecastQuantity, template.DisplayStatus, false, addId, template.SellStradgy, template.Price, template.DiscountedPrice);
                    // 遍历模板配方
                    foreach (var temp in template.FormulaInfos)
                    {
                        // 目标设备饮品添加配方
                        curInfo.AddFormulaInfo(new FormulaInfo(curInfo.Id, temp.MaterialBoxId, temp.MaterialBoxName, temp.Sort, temp.FormulaType, temp.Specs));
                    }

                    // 更新Code信息
                    //var isHaveCode = await context.BeverageInfo.AsQueryable().AnyAsync(a => a.DeviceId == item.deviceId && a.Code == template.Code);
                    if (template.CodeIsShow)
                    {
                        curInfo.AddCodeNotId(template.Code);
                    }
                    else
                    {
                        curInfo.AddCodeNotId(addId.ToString());
                        newSku = addId;
                    }

                    curInfo.UpdateCodeIsShow(template.CodeIsShow);

                    addBeverageInfo.Add(curInfo);

                }
                // 替换饮品
                else
                {
                    var curTargetBeverage = targetBeverageList.First(w => w.Id == item.beverageIds[0]);
                    //当前目标饮品
                    //foreach (var beverageId in item.beverageIds)
                    //{
                    //当前目标饮品
                    //var curTargetBeverage = targetBeverageList.First(w => w.Id == beverageId);
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

                    //开始替换内容
                    switch (item.contentType)
                    {
                        //替换基本信息
                        case ReplaceContentType.BaseInfo:
                            beverageIdsForFiles.Add(curTargetBeverage.Id);
                            curTargetBeverage.Update(template.Name, template.BeverageIcon, template.Temperature, template.Remarks, template.ProductionForecast, null, template.DisplayStatus, template.SellStradgy);
                            if (template.CodeIsShow)
                            {
                                curTargetBeverage.AddCodeNotId(template.Code);
                                // 替换部分信息记录SKU
                                sku = curTargetBeverage.Code;
                            }
                            else
                            {
                                curTargetBeverage.AddCodeNotId(curTargetBeverage.Id.ToString());
                                // 替换部分信息记录SKU
                                sku = curTargetBeverage.Id.ToString();
                            }
                            //更新Code信息
                            curTargetBeverage.UpdateCodeIsShow(template.CodeIsShow);
                            curTargetBeverage.UpdatePriceInfo(template.Price, template.DiscountedPrice);

                            break;
                        //替换配方信息
                        case ReplaceContentType.FormulaInfo:
                            curTargetBeverage.FormulaInfos.Clear();
                            //遍历模板配方
                            foreach (var temp in template.FormulaInfos)
                            {
                                //目标设备饮品添加配方
                                curTargetBeverage.AddFormulaInfo(new FormulaInfo(curTargetBeverage.Id, temp.MaterialBoxId, temp.MaterialBoxName, temp.Sort, temp.FormulaType, temp.Specs));
                            }
                            curTargetBeverage.UpdateForecastQuantity(template.ForecastQuantity);
                            //替换部分信息记录SKU
                            sku = curTargetBeverage.Code;
                            break;
                        //替换全部
                        case ReplaceContentType.ALL:
                            beverageIdsForFiles.Add(curTargetBeverage.Id);
                            curTargetBeverage.Update(template.Name, template.BeverageIcon, template.Temperature, template.Remarks, template.ProductionForecast, template.ForecastQuantity, template.DisplayStatus, template.SellStradgy);
                            //if (template.CodeIsShow)
                            //    curTargetBeverage.AddCodeNotId(template.Code);
                            if (template.CodeIsShow)
                            {
                                curTargetBeverage.AddCodeNotId(template.Code);
                                // 替换部分信息记录SKU
                                sku = curTargetBeverage.Code;
                            }
                            else
                            {
                                curTargetBeverage.AddCodeNotId(curTargetBeverage.Id.ToString());
                                // 替换部分信息记录SKU
                                sku = curTargetBeverage.Id.ToString();
                            }
                            //更新Code信息
                            curTargetBeverage.UpdateCodeIsShow(template.CodeIsShow);

                            curTargetBeverage.UpdatePriceInfo(template.Price, template.DiscountedPrice);

                            //更新Code
                            curTargetBeverage.AddCodeNotId(template.Code);
                            if (curTargetBeverage.FormulaInfos != null)
                                curTargetBeverage.FormulaInfos.Clear();
                            foreach (var temp in template.FormulaInfos)
                            {
                                //目标设备饮品添加配方
                                curTargetBeverage.AddFormulaInfo(new FormulaInfo(curTargetBeverage.Id, temp.MaterialBoxId, temp.MaterialBoxName, temp.Sort, temp.FormulaType, temp.Specs));
                            }
                            break;
                        default:
                            break;
                            //}
                    }

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

            //如果有新增饮品
            if (addBeverageInfo.Count > 0)
            {
                await context.AddRangeAsync(addBeverageInfo);
            }

            //如果有替换饮品
            if (targetBeverageList.Count > 0)
            {
                context.BeverageInfo.UpdateRange(targetBeverageList);

                // todo:后续尝试去掉saveChanges
                var res = await context.SaveChangesAsync();
            }

            // 绑定文件关系
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

            return downSendDto;
        }

        // 深拷贝配方信息
        private List<FormulaInfo> DeepCloneFormulaInfos(List<FormulaInfo> originalFormulaInfos, long boxId)
        {
            return originalFormulaInfos.Select(f => new FormulaInfo(f.BeverageInfoId, boxId, f.MaterialBoxName, f.Sort, f.FormulaType, f.Specs)).ToList();
        }
    }
}