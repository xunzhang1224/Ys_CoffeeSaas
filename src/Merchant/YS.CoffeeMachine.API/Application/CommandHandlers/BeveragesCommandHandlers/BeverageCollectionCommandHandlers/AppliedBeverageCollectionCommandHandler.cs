using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text.Json;
using Yitter.IdGenerator;
using YS.CoffeeMachine.API.Utils;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageCollectionCommands;
using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YS.CoffeeMachine.Application.Dtos.Cap;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Domain.AggregatesModel.Files;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.BeveragesCommandHandlers.BeverageCollectionCommandHandlers
{
    /// <summary>
    /// 饮品合集应用到设备
    /// </summary>
    /// <param name="context"></param>
    public class AppliedBeverageCollectionCommandHandler(CoffeeMachineDbContext context, IMapper mapper) : ICommandHandler<AppliedBeverageCollectionCommand, CommandDownSends>
    {
        /// <summary>
        /// 饮品合集应用到设备
        /// </summary>
        public async Task<CommandDownSends> Handle(AppliedBeverageCollectionCommand request, CancellationToken cancellationToken)
        {
            //获取所有目标设备
            var deviceInfos = await context.DeviceInfo
                              .Include(i => i.BeverageInfos)
                              .Include(i => i.SettingInfo)
                              .ThenInclude(s => s.MaterialBoxs)
                              .Where(w => request.beverageCollectionInput.deviceIds.Contains(w.Id)).ToListAsync();
            if (deviceInfos.Count == 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0008)]);
            //获取目标设备的Id列表
            var targetDeviceIds = deviceInfos.Select(s => s.DeviceBaseId).Distinct().ToList();
            var targetBaseDeviceMids = await context.DeviceBaseInfo.AsQueryable().AsNoTracking()
                .Where(w => targetDeviceIds.Contains(w.Id))
                .Select(s => s.Mid)
                .ToListAsync();
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

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };

            var newBeverageIds = new Dictionary<long, string>();

            var sourceBevergeList = new List<BeverageInfo>();
            foreach (var item in deviceInfos)
            {
                //新增饮品列表
                var needAddBeverageInfo = new List<BeverageInfo>();
                //饮品集合应用到设备时，设备原有的饮品全部清除
                if (item.BeverageInfos != null && item.BeverageInfos.Count > 0)
                {
                    //删除非饮品集合的历史记录
                    foreach (var bitem in item.BeverageInfos)
                    {
                        bitem.IsDelete = true;
                        bitem.AddCodeNotId(bitem.Id.ToString());
                    }
                }

                //遍历饮品集合的饮品，依次添加到设备饮品中
                foreach (var versions in beverageVersions)
                {
                    var curVersionsInfo = JsonSerializer.Deserialize<BeverageInfoDto>(versions.BeverageInfoDataString, options);
                    if (curVersionsInfo == null)
                        throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0048)]);

                    var addId = YitIdHelper.NextId();
                    newBeverageIds[addId] = curVersionsInfo.BeverageIcon;

                    //var nId = YitIdHelper.NextId();
                    var curBeverageInfo = new BeverageInfo(item.Id, curVersionsInfo.Name, curVersionsInfo.BeverageIcon, curVersionsInfo.temperature, curVersionsInfo.Remarks, curVersionsInfo.ProductionForecast, curVersionsInfo.ForecastQuantity, curVersionsInfo.DisplayStatus, false, addId, curVersionsInfo.SellStradgy, curVersionsInfo.Price ?? 0, curVersionsInfo.DiscountedPrice ?? 0);
                    curBeverageInfo.SetId(addId);
                    curBeverageInfo.UpdateSort(curVersionsInfo.Sort ?? 0);
                    if (!curVersionsInfo.CodeIsShow)
                    {
                        curBeverageInfo.AddCodeNotId(addId.ToString());// curVersionsInfo.Code------已优化，改成当前饮品Id
                        curBeverageInfo.UpdateCodeIsShow(false);// curVersionsInfo.CodeIsShow------已优化
                    }
                    else
                    {
                        curBeverageInfo.AddCodeNotId(curVersionsInfo.Code);
                        curBeverageInfo.UpdateCodeIsShow(curVersionsInfo.CodeIsShow);
                    }
                    foreach (var f_info in curVersionsInfo.FormulaInfos)
                    {
                        curBeverageInfo.AddFormulaInfo(new FormulaInfo(curBeverageInfo.Id, f_info.MaterialBoxId, f_info.MaterialBoxName, f_info.Sort, f_info.FormulaType, f_info.Specs));
                    }
                    //新增饮品集合
                    needAddBeverageInfo.Add(curBeverageInfo);
                }
                item.BeverageInfos.AddRange(needAddBeverageInfo);
                if (sourceBevergeList == null || sourceBevergeList.Count == 0)
                {
                    sourceBevergeList.AddRange(needAddBeverageInfo);
                }
            }

            #region 添加饮品图片关联关系

            List<string> beverageIcons = sourceBevergeList.Select(s => s.BeverageIcon).Distinct().ToList();

            // 获取所有目标饮品
            var targetDeviceInfoIds = deviceInfos.Select(s => s.Id).Distinct().ToList();
            var targetBevergeList = await context.BeverageInfo.Where(w => targetDeviceInfoIds.Contains(w.DeviceId)).ToListAsync();

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

            //await context.SaveChangesAsync();
            //string jsonData = JsonSerializer.Serialize(sourceBevergeList, new JsonSerializerOptions
            //{
            //    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // 禁用特殊字符的转义
            //    WriteIndented = true // 美化输出（可选）
            //});
            //var aa = JsonSerializer.Serialize(new BasicClassHelper().GetBeverages(sourceBevergeList));
            //组装下发饮品信息
            var downSendDto = new CommandDownSends()
            {
                Method = "9026",
                Mid = targetBaseDeviceMids[0],  //"饮品合集",
                Mids = targetBaseDeviceMids,
                Params = JsonSerializer.Serialize(new BasicClassHelper().GetBeverages(sourceBevergeList)),
                IsRecordLog = true
            };

            return downSendDto;
        }
    }
}