using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula;
using Yitter.IdGenerator;
using YS.CoffeeMachine.API.Utils;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.BeverageWarehouse;
using YS.CoffeeMachine.Domain.AggregatesModel.Files;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YS.Provider.OSS.Interface.Base;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.BeveragesCommandHandlers.BeverageInfoCommandHandlers
{
    /// <summary>
    /// 饮品添加到饮品库
    /// </summary>
    /// <param name="context"></param>
    public class AddBeverageInfoTemplateCommandHandler(CoffeeMachineDbContext context) : ICommandHandler<AddBeverageInfoTemplateCommand, bool>
    {
        /// <summary>
        /// 饮品添加到饮品库
        /// </summary>
        public async Task<bool> Handle(AddBeverageInfoTemplateCommand request, CancellationToken cancellationToken)
        {
            var addId = YitIdHelper.NextId();
            var info = await context.BeverageInfo.Include(i => i.FormulaInfos).ThenInclude(ti => ti.MaterialBox).FirstAsync(w => w.Id == request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

            // 为空的code不参与校验 可同时存在为空的code
            if (!string.IsNullOrEmpty(info.Code) && info.CodeIsShow)
            {
                var templates = await context.BeverageInfoTemplate.Where(w => w.Code == info.Code && info.CodeIsShow).ToListAsync();
                //如果饮品库存在同Sku饮品，则清除掉饮品库的，将当前饮品添加
                if (templates.Count > 0)
                    context.RemoveRange(templates);
            }

            var deviceInfo = await context.DeviceInfo.FirstOrDefaultAsync(w => w.Id == info.DeviceId);
            if (deviceInfo == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0008)]);

            // 提前生成饮品库的饮品id
            long id = YitIdHelper.NextId();

            // 文件操作
            var fileId = await context.FileManage.AsQueryable().Where(w => w.FilePath == info.BeverageIcon).Select(s => s.Id).FirstOrDefaultAsync();
            if (fileId != null)
            {
                //var fileOpt = new FileOptionUtils(context, _oSSService);
                var fileRelation = new FileRelation(fileId, id, 3);
                await context.AddAsync(fileRelation);
                //await fileOpt.CreateFileRelation(fileId, id, 3);
            }

            var template = new BeverageInfoTemplate(request.enterpriseInfoId, deviceInfo.DeviceModelId, info.Name, info.BeverageIcon, info.Temperature, info.Remarks, info.ProductionForecast, info.ForecastQuantity ?? 0, info.DisplayStatus, info.SellStradgy, id, categoryIds: info.CategoryIds);

            // 设置饮品Code信息
            template.AddCodeNotId(!info.CodeIsShow ? addId.ToString() : info.Code);
            template.UpdateCodeIsShow(info.CodeIsShow);
            var materialBoxs = await context.DeviceMaterialInfo.Where(w => w.DeviceBaseId == deviceInfo.DeviceBaseId && w.Type == MaterialTypeEnum.Cassette).ToListAsync();
            foreach (var item in info.FormulaInfos)
            {
                long? boxId = item.MaterialBoxId; //item.MaterialBox == null ? null : item.MaterialBox.Sort;
                // 当前饮品的物料盒信息
                var boxName = materialBoxs.FirstOrDefault(w => w.Index == item.MaterialBoxId)?.Name ?? string.Empty;// 根据物料信息表获取料盒名称
                template.FormulaInfoTemplates.Add(new FormulaInfoTemplate(template.Id, boxId, boxName, item.Sort, item.FormulaType, item.Specs));
            }
            var res = await context.BeverageInfoTemplate.AddAsync(template);
            //var curCode = info.Code.Replace(info.TransId.ToString(), "");
            //template.AddCodeNotId(info.Code);
            return res != null;
        }
    }
}