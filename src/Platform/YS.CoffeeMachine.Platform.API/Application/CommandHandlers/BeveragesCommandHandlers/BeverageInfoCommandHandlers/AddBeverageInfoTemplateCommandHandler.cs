using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.BeverageWarehouse;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.BeveragesCommandHandlers.BeverageInfoCommandHandlers
{
    /// <summary>
    /// 饮品添加到饮品库
    /// </summary>
    /// <param name="context"></param>
    public class AddBeverageInfoTemplateCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<AddBeverageInfoTemplateCommand, bool>
    {
        /// <summary>
        /// 饮品添加到饮品库
        /// </summary>
        public async Task<bool> Handle(AddBeverageInfoTemplateCommand request, CancellationToken cancellationToken)
        {
            var info = await context.BeverageInfo.Include(i => i.FormulaInfos).ThenInclude(ti => ti.MaterialBox).FirstAsync(w => w.Id == request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            var templates = await context.BeverageInfoTemplate.Where(w => w.Code == info.Code).ToListAsync();
            //如果饮品库存在同Sku饮品，则清除掉饮品库的，将当前饮品添加
            if (templates.Count > 0)
                context.RemoveRange(templates);
            var deviceInfo = await context.DeviceInfo.FirstOrDefaultAsync(w => w.Id == info.DeviceId);
            if (deviceInfo == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0008)]);
            var template = new BeverageInfoTemplate(request.enterpriseInfoId, deviceInfo.DeviceModelId, info.Name, info.BeverageIcon, info.Temperature, info.Remarks, info.ProductionForecast, info.ForecastQuantity, info.DisplayStatus);
            template.AddCodeNotId(info.Code);
            template.UpdateCodeIsShow(info.CodeIsShow);
            foreach (var item in info.FormulaInfos)
            {
                long? boxId = item.MaterialBox == null ? null : item.MaterialBox.Sort;
                template.FormulaInfoTemplates.Add(new FormulaInfoTemplate(template.Id, boxId, string.Empty, item.Sort, item.FormulaType, item.Specs));
            }
            var res = await context.BeverageInfoTemplate.AddAsync(template);
            //var curCode = info.Code.Replace(info.Id.ToString(), "");
            //template.AddCodeNotId(info.Code);
            return res != null;
        }
    }
}