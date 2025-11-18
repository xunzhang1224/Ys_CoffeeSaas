using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text.Json;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoTemplateCommands;
using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YS.CoffeeMachine.Domain.AggregatesModel.BeverageWarehouse;
using YS.CoffeeMachine.Domain.IPlatformRepositories.BeverageWarehouseRepositorys;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.BeveragesCommandHandlers.BeverageInfoTemplateCommandHandlers
{
    /// <summary>
    /// 编辑饮品库模型
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="context"></param>
    public class UpdateBeverageInfoTemplateCommandHandler(IPBeverageInfoTemplateRepository repository, CoffeeMachinePlatformDbContext context) : ICommandHandler<UpdateBeverageInfoTemplateCommand, bool>
    {
        /// <summary>
        /// 编辑饮品库模型
        /// </summary>
        public async Task<bool> Handle(UpdateBeverageInfoTemplateCommand request, CancellationToken cancellationToken)
        {
            var info = await context.BeverageInfoTemplate.Include(i => i.FormulaInfoTemplates).FirstAsync(w => w.Id == request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            info.Update(request.name, request.beverageIcon, request.temperature, request.remarks, request.productionForecast, request.forecastQuantity, request.displayStatus);
            var formulaInfoTemplates = new List<FormulaInfoTemplate>();
            //var hasDefaluMaterial = false;
            foreach (var item in request.formulaInfoTemplateDto)
            {
                if (!FormulaSpecsConverter.ValidateSpecsJson(item.FormulaType, item.Specs.ToString()))
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0056)]);
                var sort = item.MaterialBoxId == null ? -1 : (int)item.MaterialBoxId;
                formulaInfoTemplates.Add(new FormulaInfoTemplate(info.Id, item.MaterialBoxId, item.MaterialBoxName, sort, item.FormulaType, item.Specs));
                //if (item.FormulaType == FormulaTypeEnum.Coffee)
                //    hasDefaluMaterial = true;
            }
            //if (!hasDefaluMaterial)
            //    throw ExceptionHelper.AppFriendly("配方缺少默认物料：咖啡");
            //添加配方列表
            info.AddRangeFormulaInfos(formulaInfoTemplates);

            // 序列化成 String
            string jsonData = JsonSerializer.Serialize(request, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // 禁用特殊字符的转义
                WriteIndented = true // 美化输出（可选）
            });
            //添加历史版本信息
            info.InsertBeverageTemplateVersions(BeverageVersionTypeEnum.Insert, jsonData);
            //持久化数据
            var res = await repository.UpdateAsync(info);
            return res != null;
        }
    }
}
