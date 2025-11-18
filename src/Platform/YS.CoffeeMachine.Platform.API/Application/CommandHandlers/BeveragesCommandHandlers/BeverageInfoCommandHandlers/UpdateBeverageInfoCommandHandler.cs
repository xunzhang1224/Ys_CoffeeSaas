using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text.Json;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoCommands;
using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Domain.IRepositories.BeveragesRepositorys;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.BeveragesCommandHandlers.BeverageInfoCommandHandlers
{
    /// <summary>
    /// 修改饮品信息
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="context"></param>
    public class UpdateBeverageInfoCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<UpdateBeverageInfoCommand, bool>
    {
        /// <summary>
        /// 修改饮品信息
        /// </summary>
        public async Task<bool> Handle(UpdateBeverageInfoCommand request, CancellationToken cancellationToken)
        {
            var info = await context.P_BeverageInfo.Include(i => i.FormulaInfos).FirstAsync(w => w.Id == request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

            //编辑饮品时，饮品Code不能与当前设备其他饮品重复
            //var hasCode = await context.BeverageInfo.FirstOrDefaultAsync(w => w.DeviceBaseId == info.DeviceBaseId && w.Id != info.Id && w.Code == curCode);
            //if (hasCode != null)
            //{
            //    throw ExceptionHelper.AppFriendly(string.Format(L.Text[nameof(ErrorCodeEnum.D0054)], info.Code));
            //}

            info.Update(request.name, request.beverageIcon, request.temperature, request.remarks, request.productionForecast, request.forecastQuantity, request.displayStatus);
            var formulaInfos = new List<P_FormulaInfo>();
            //var hasDefaluMaterial = false;
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
            info.InsertBeverageVersions(BeverageVersionTypeEnum.Edit, jsonData);
            var res = context.Update(info);
            //context.SaveChanges();
            return res != null;
        }
    }
}
