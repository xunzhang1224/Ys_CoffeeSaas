using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text.Json;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoCommands;
using YS.CoffeeMachine.Application.Dtos.BeverageDots;
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
    /// 添加饮品
    /// </summary>
    /// <param name="context"></param>
    public class CreateBeverageInfoCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<CreateBeverageInfoCommand, bool>
    {
        /// <summary>
        /// 添加饮品
        /// </summary>
        public async Task<bool> Handle(CreateBeverageInfoCommand request, CancellationToken cancellationToken)
        {
            var info = new P_BeverageInfo(request.deviceModelId, request.name, request.beverageIcon, request.temperature, request.remarks, request.productionForecast
                , request.forecastQuantity, request.displayStatus, false, request.languageKey);
            if (!string.IsNullOrWhiteSpace(request.code))
            {
                var hasCode = await context.P_BeverageInfo.FirstOrDefaultAsync(w => w.Code == request.code);
                if (hasCode != null)
                {
                    throw ExceptionHelper.AppFriendly(string.Format(L.Text[nameof(ErrorCodeEnum.D0054)], info.Code));
                }
                info.AddCodeNotId(request.code);
            }
            var formulaInfos = new List<P_FormulaInfo>();
            //var hasDefaluMaterial = false;
            if (request.formulaInfos == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0055)]);
            foreach (var item in request.formulaInfos)
            {
                if (!FormulaSpecsConverter.ValidateSpecsJson(item.FormulaType, item.Specs.ToString()))
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0056)]);
                formulaInfos.Add(new P_FormulaInfo(info.Id, item.MaterialBoxId, item.MaterialBoxName, item.Sort, item.FormulaType, item.Specs));
                //if (item.FormulaType == FormulaTypeEnum.Coffee)
                //    hasDefaluMaterial = true;
            }
            //if (!hasDefaluMaterial)
            //    throw ExceptionHelper.AppFriendly("配方缺少默认物料：咖啡");

            //添加配方列表
            info.AddRangeFormulaInfos(formulaInfos);

            // 序列化成 String
            string jsonData = JsonSerializer.Serialize(request, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // 禁用特殊字符的转义
                WriteIndented = true // 美化输出（可选）
            });
            //添加历史版本信息
            info.InsertBeverageVersions(BeverageVersionTypeEnum.Insert, jsonData);
            await context.AddAsync(info);

            //如果新增饮品code为空，则默认code为当前Id
            //if (string.IsNullOrWhiteSpace(request.code))
            //    info.AddCode(request.code);

            return true;
        }
    }
}
