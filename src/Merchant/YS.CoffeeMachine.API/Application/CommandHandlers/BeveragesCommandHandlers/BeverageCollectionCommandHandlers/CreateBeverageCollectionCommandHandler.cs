using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text.Json;
using Yitter.IdGenerator;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageCollectionCommands;
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
    /// 添加饮品集合
    /// </summary>
    /// <param name="context"></param>
    public class CreateBeverageCollectionCommandHandler(CoffeeMachineDbContext context) : ICommandHandler<CreateBeverageCollectionCommand, bool>
    {
        /// <summary>
        /// 添加饮品集合
        /// </summary>
        public async Task<bool> Handle(CreateBeverageCollectionCommand request, CancellationToken cancellationToken)
        {
            //获取设备信息
            var deviceInfo = await context.DeviceInfo.IgnoreQueryFilters().AsNoTracking().FirstOrDefaultAsync(w => !w.IsDelete && w.Id == request.deviceId);
            if (deviceInfo == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0008)]);
            var beverages = await context.BeverageInfo.IgnoreQueryFilters().Include(i => i.FormulaInfos)/*.Include(i => i.BeverageVersions)*/.Where(w => !w.IsDelete && w.DeviceId == request.deviceId).OrderBy(o => o.Sort).ToListAsync();
            var beverageIds = beverages.Select(s => new { s.Id, s.Name, s.BeverageIcon }).ToList();
            if (beverageIds == null || beverageIds.Count == 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0049)]);

            // 检查合集名称是否已存在
            var allbeverageCollectionCount = await context.BeverageCollection.Where(w => w.EnterpriseInfoId == request.enterpriseInfoId && w.Name == request.name).CountAsync();
            if (allbeverageCollectionCount > 0)
                throw ExceptionHelper.AppFriendly(string.Format(L.Text[nameof(ErrorCodeEnum.D0050)], request.name));

            // 需新增的历史Id 对应 图片地址
            var newBeverageIds = new Dictionary<long, string>();

            // 将当前设备饮品批量保存历史记录
            foreach (var beverage in beverages)
            {
                var addId = YitIdHelper.NextId();

                // 序列化成 String
                string jsonData = JsonSerializer.Serialize(beverage, new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // 禁用特殊字符的转义
                    WriteIndented = true // 美化输出（可选）
                });
                beverage.InsertBeverageVersions(BeverageVersionTypeEnum.Collection, jsonData, addId);
                newBeverageIds[addId] = beverage.BeverageIcon;
            }

            #region 图片处理
            var beverageIcons = beverageIds.Select(s => s.BeverageIcon).Distinct().ToList();
            var files = await context.FileManage.AsQueryable().Where(w => beverageIcons.Contains(w.FilePath)).ToArrayAsync();

            if (files != null && files.Count() > 0)
            {
                var fileDics = files.ToDictionary(t => t.FilePath, t => t.Id);
                var fileRelations = new List<FileRelation>();
                foreach (var item in newBeverageIds)
                {
                    fileDics.TryGetValue(item.Value, out var fileId);
                    if (fileId != null && fileId > 0)
                    {
                        var fileRelation = new FileRelation(fileId, item.Key, 2);
                        fileRelations.Add(fileRelation);
                    }
                }

                if (fileRelations.Count > 0)
                {
                    await context.AddRangeAsync(fileRelations);
                }
            }
            #endregion

            //context

            //// 提交饮品历史记录
            //var res = await context.SaveChangesAsync();
            //// 获取当前饮品的历史记录Id集合
            //var bVeasions = beverages.SelectMany(s => s.BeverageVersions).GroupBy(g => g.BeverageInfoId).ToList();
            //var versionsIds = new List<long>();
            //foreach (var item in bVeasions)
            //{
            //    versionsIds.Add(item.ToList().OrderByDescending(o => o.CreateTime).First().Id);
            //}

            var info = new BeverageCollection(request.enterpriseInfoId, deviceInfo.DeviceModelId, request.name, string.Join(",", newBeverageIds.Keys), string.Join(",", beverageIds.Select(s => s.Name)));
            //添加饮品集合
            await context.BeverageCollection.AddAsync(info);
            return true;
        }
    }
}
