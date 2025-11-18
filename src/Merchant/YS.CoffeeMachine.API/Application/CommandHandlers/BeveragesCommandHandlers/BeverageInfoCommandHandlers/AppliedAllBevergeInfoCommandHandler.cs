using Microsoft.EntityFrameworkCore;
using System.Text.Json;
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
    /// 应用指定设备所有饮品信息
    /// </summary>
    /// <param name="context"></param>
    public class AppliedAllBevergeInfoCommandHandler(CoffeeMachineDbContext context) : ICommandHandler<AppliedAllBevergeInfoCommand, CommandDownSends>
    {
        /// <summary>
        /// 应用指定设备所有饮品信息
        /// </summary>
        public async Task<CommandDownSends> Handle(AppliedAllBevergeInfoCommand request, CancellationToken cancellationToken)
        {
            if (request.targetDeviceIds.Contains(request.deviceId))
                throw ExceptionHelper.AppFriendly(string.Format(L.Text[nameof(ErrorCodeEnum.D0051)], request.deviceId));
            //获取源设备饮品
            var sourceBevergeList = await context.BeverageInfo.Include(i => i.FormulaInfos).Where(w => w.DeviceId == request.deviceId).ToListAsync();
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

            //获取原设备信息
            var sourceDevice = await context.DeviceInfo.AsQueryable().AsNoTracking().FirstOrDefaultAsync(w => w.Id == request.deviceId);
            if (sourceDevice == null)
                throw ExceptionHelper.AppFriendly(string.Format(L.Text[nameof(ErrorCodeEnum.D0005)], request.deviceId));
            var baseDeviceInfo = await context.DeviceBaseInfo.AsQueryable().AsNoTracking().FirstOrDefaultAsync(w => w.Id == sourceDevice.DeviceBaseId);
            if (baseDeviceInfo == null)
                throw ExceptionHelper.AppFriendly(string.Format(L.Text[nameof(ErrorCodeEnum.D0005)], sourceDevice.DeviceBaseId));

            //获取目标设备信息
            var targetDeviceBaseIds = await context.DeviceInfo.AsQueryable().AsNoTracking().Where(w => request.targetDeviceIds.Contains(w.Id) && w.DeviceBaseId != 0).Select(s => s.DeviceBaseId).ToListAsync();
            if (targetDeviceBaseIds.Count == 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0008)]);
            var targetBaseDeviceMids = await context.DeviceBaseInfo.AsQueryable().AsNoTracking()
                .Where(w => targetDeviceBaseIds.Contains(w.Id))
                .Select(s => s.Mid)
                .ToListAsync();
            if (targetBaseDeviceMids == null || targetBaseDeviceMids.Count == 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0008)]);

            //组装下发饮品信息
            var downSendDto = new CommandDownSends()
            {
                Method = "9026",
                Mid = baseDeviceInfo.Mid,
                Mids = targetBaseDeviceMids,
                IsRecordLog = true
            };

            //根据饮品获取设备料盒信息
            //var deviceIds = targetBevergeList.Select(s => s.DeviceId).Distinct().ToList();
            //var deviceDicList = await context.DeviceInfo
            //                    .Include(i => i.SettingInfo)
            //                        .ThenInclude(s => s.MaterialBoxs)
            //                    .Where(w => deviceIds.Contains(w.Id))
            //                    .Select(device => new
            //                    {
            //                        DeviceId = device.Id,
            //                        device.SettingInfo.MaterialBoxs
            //                    })
            //                    .ToDictionaryAsync(
            //                        x => x.DeviceId,
            //                        x => x.MaterialBoxs.ToList()
            //                    );

            var newBeverageIds = new Dictionary<long, string>();
            List<string> beverageIcons = sourceBevergeList.Select(s => s.BeverageIcon).Distinct().ToList();
            // 需要添加的饮品
            var addBeverageInfo = new List<BeverageInfo>();
            foreach (var item in request.targetDeviceIds)
            {
                foreach (var template in sourceBevergeList)
                {
                    var id = YitIdHelper.NextId();
                    newBeverageIds[id] = template.BeverageIcon;
                    var curInfo = new BeverageInfo(item, template.Name, template.BeverageIcon, template.Temperature, template.Remarks, template.ProductionForecast,
                        template.ForecastQuantity, template.DisplayStatus, false, id, template.SellStradgy);
                    curInfo.SetId(id);
                    //更新Code
                    curInfo.AddCodeNotId(id.ToString());
                    // Code显示
                    curInfo.UpdateCodeIsShow(template.CodeIsShow);
                    curInfo.UpdateSort(template.Sort ?? 0);
                    curInfo.UpdatePrice(template.Price, template.DiscountedPrice);// 同步价格

                    // 遍历模板配方
                    foreach (var temp in template.FormulaInfos)
                    {
                        //long? materialBoxId = null;
                        //if (temp.MaterialBox != null)
                        //{
                        //    //根据配方模板料盒位置，获取当前设备同位置料盒Id
                        //    var cBox = deviceDicList[item].FirstOrDefault(w => w.Sort == temp.MaterialBox.Sort);
                        //    //当前料盒Id（可以为null）
                        //    materialBoxId = cBox?.Id ?? null;
                        //}

                        //目标设备饮品添加配方
                        curInfo.AddFormulaInfo(new FormulaInfo(curInfo.Id, temp.MaterialBoxId, temp.MaterialBoxName, temp.Sort, temp.FormulaType, temp.Specs));
                    }

                    //添加配方信息
                    //curInfo.AddRangeFormulaInfos(template.FormulaInfos);
                    addBeverageInfo.Add(curInfo);
                }
            }

            #region 添加饮品图片关联关系

            // targetBevergeList  首先要删除原有设备内饮品相关的图片关联关系
            if (targetBevergeList.Count > 0)
            {
                var targetVevergeIds = targetBevergeList.Select(s => s.Id).ToList();
                var beverageVersionIds = await context.BeverageVersion.AsQueryable().Where(w => targetVevergeIds.Contains(w.BeverageInfoId) && w.VersionType != BeverageVersionTypeEnum.Collection).Select(s => s.Id).ToListAsync();
                var allIds = targetVevergeIds.Concat(beverageVersionIds).ToList();
                context.FileRelation.RemoveRange(context.FileRelation.Where(w => allIds.Contains(w.TargetId)));
            }

            var fileDics = await context.FileManage.AsQueryable().Where(w => beverageIcons.Contains(w.FilePath)).ToDictionaryAsync(t => t.FilePath, t => t.Id);
            if (fileDics != null && fileDics.Count > 0)
            {
                var fileRelations = new List<FileRelation>();
                foreach (var item in newBeverageIds)
                {
                    fileDics.TryGetValue(item.Value, out var fileId);
                    if (fileId != null && fileId > 0)
                    {
                        var fileRelation = new FileRelation(fileId, item.Key, 1);
                        fileRelations.Add(fileRelation);
                    }
                }

                if (fileRelations.Count > 0)
                {
                    await context.AddRangeAsync(fileRelations);
                }
            }
            #endregion

            if (addBeverageInfo.Count > 0)
            {
                await context.AddRangeAsync(addBeverageInfo);
                downSendDto.Params = JsonSerializer.Serialize(new BasicClassHelper().GetBeverages(addBeverageInfo));
            }

            return downSendDto;
        }
    }
}