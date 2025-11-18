using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageCollectionCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.BeveragesCommandHandlers.BeverageCollectionCommandHandlers
{
    /// <summary>
    /// 创建饮品合集
    /// </summary>
    /// <param name="context"></param>
    public class CreateP_BeverageCollectionCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<CreateP_BeverageCollectionCommand, bool>
    {
        /// <summary>
        /// 创建饮品合集
        /// </summary>
        public async Task<bool> Handle(CreateP_BeverageCollectionCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.languageKey))
                throw ExceptionHelper.AppFriendly("未选择语言");
            if (request.beverageInfoIds == null || request.beverageInfoIds.Count == 0)
                throw ExceptionHelper.AppFriendly("未选择饮品");

            // 检查合集名称是否已存在
            var allbeverageCollectionCount = await context.P_BeverageCollection.Where(w => !w.IsDelete && w.Name == request.name).CountAsync();
            if (allbeverageCollectionCount > 0)
                throw ExceptionHelper.AppFriendly(string.Format(L.Text[nameof(ErrorCodeEnum.D0050)], request.name));

            var beverages = await context.P_BeverageInfo.IgnoreQueryFilters().Include(i => i.FormulaInfos)/*.Include(i => i.BeverageVersions)*/
                .Where(w => request.beverageInfoIds.Contains(w.Id))
                .ToListAsync();

            var checkCount = beverages.Where(a => a.DeviceModelId != request.deviceModelId || a.LanguageKey != request.languageKey).Count();
            if (checkCount > 0)
                throw ExceptionHelper.AppFriendly("选择的饮品需要和饮品集合所选的语言和设备型号一致");

            // 将当前设备饮品批量保存历史记录
            foreach (var beverage in beverages)
            {
                // 序列化成 String
                string jsonData = JsonSerializer.Serialize(beverage, new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // 禁用特殊字符的转义
                    WriteIndented = true // 美化输出（可选）
                });
                beverage.InsertBeverageVersions(BeverageVersionTypeEnum.Collection, jsonData);
            }

            // 提交饮品历史记录
            var res = await context.SaveChangesAsync();

            // 获取当前饮品的历史记录Id集合
            var bVeasions = beverages.SelectMany(s => s.BeverageVersions).GroupBy(g => g.BeverageInfoId).ToList();
            var versionsIds = new List<long>();
            foreach (var item in bVeasions)
            {
                versionsIds.Add(item.ToList().OrderByDescending(o => o.CreateTime).First().Id);
            }

            var info = new P_BeverageCollection(request.languageKey, request.deviceModelId, request.name, string.Join(",", versionsIds.Select(s => s)), string.Join(",", beverages.Select(s => s.Name)));
            // 添加饮品集合
            await context.P_BeverageCollection.AddAsync(info);

            return true;
        }
    }
}
