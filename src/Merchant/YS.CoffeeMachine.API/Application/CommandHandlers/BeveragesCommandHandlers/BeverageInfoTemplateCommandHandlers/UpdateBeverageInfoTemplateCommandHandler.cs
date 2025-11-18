using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text.Json;
using Yitter.IdGenerator;
using YS.CoffeeMachine.API.Utils;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoCommands;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoTemplateCommands;
using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YS.CoffeeMachine.Domain.AggregatesModel.BeverageWarehouse;
using YS.CoffeeMachine.Domain.IRepositories.BeverageWarehouseRepositorys;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YS.Provider.OSS.Interface.Base;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.BeveragesCommandHandlers.BeverageInfoTemplateCommandHandlers
{
    /// <summary>
    /// 编辑饮品库模型
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="context"></param>
    public class UpdateBeverageInfoTemplateCommandHandler(IBeverageInfoTemplateRepository repository, CoffeeMachineDbContext context, IOSSService _oSSService, UserHttpContext _user) : ICommandHandler<UpdateBeverageInfoTemplateCommand, bool>
    {
        /// <summary>
        /// 编辑饮品库模型
        /// </summary>
        public async Task<bool> Handle(UpdateBeverageInfoTemplateCommand request, CancellationToken cancellationToken)
        {
            var info = await context.BeverageInfoTemplate.Include(i => i.FormulaInfoTemplates).FirstAsync(w => w.Id == request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

            var exists = await context.BeverageInfoTemplate.AsQueryable().AnyAsync(a => !string.IsNullOrEmpty(a.Code) && a.Code == request.code && a.Id != request.id && a.EnterpriseInfoId == _user.TenantId);

            if (exists)
                throw ExceptionHelper.AppFriendly(string.Format(L.Text[nameof(ErrorCodeEnum.D0054)], request.code));

            #region 图片操作

            var fileOpt = new FileOptionUtils(context, _oSSService);

            // 图片操作
            string newFilePath = string.Empty;
            long fileId = 0;

            var fileRelationInfo = await context.FileRelation.AsQueryable().Where(w => w.TargetId == request.id).FirstOrDefaultAsync();

            // 复制oss文件  并且创建饮品文件关系
            if (request.file != null)
            {
                var fileDic = await fileOpt.FileMove(request.file, request.beverageIcon, 1, request.id);
                fileId = fileDic.Keys.First();
                newFilePath = fileDic[fileId];
            }
            else
            {
                // 如果是用的资源库的图片 或者没有修改图片
                fileId = await context.FileManage.AsQueryable().Where(w => w.FilePath == request.beverageIcon).Select(s => s.Id).FirstOrDefaultAsync();
            }
            if (fileId > 0)
                fileRelationInfo.UpdateFileId(fileId);

            newFilePath = string.IsNullOrWhiteSpace(newFilePath) ? request.beverageIcon : newFilePath;
            #endregion

            info.Update(request.name, request.beverageIcon, request.temperature, request.remarks, request.productionForecast, request.forecastQuantity, request.displayStatus, request.sellStradgy, categoryIds: request.productCategoryIds);
            if (!string.IsNullOrWhiteSpace(request.code))
            {
                info.AddCodeNotId(request.code);
                info.UpdateCodeIsShow(true);
            }
            else
            {
                info.AddCode(string.Empty);
                info.UpdateCodeIsShow(false);
            }
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

            // 判断是否跟换图片做相应的处理
            long beverageVersionId = YitIdHelper.NextId();
            if (request.file != null)
            {
                // 插入最新的oss文件地址
                var tt = JsonSerializer.Deserialize<CreateBeverageInfoCommandInput>(jsonData);
                tt.beverageIcon = newFilePath;
                jsonData = JsonSerializer.Serialize(tt, new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // 禁用特殊字符的转义
                    WriteIndented = true // 美化输出（可选）
                });
            }

            // 给饮品历史版本添加文件关联
            await fileOpt.CreateFileRelation(fileId, beverageVersionId, 4);
            info.InsertBeverageTemplateVersions(BeverageVersionTypeEnum.Edit, jsonData, beverageVersionId);

            //添加历史版本信息
            //info.InsertBeverageTemplateVersions(BeverageVersionTypeEnum.Insert, jsonData);
            //持久化数据
            var res = await repository.UpdateAsync(info);
            return res != null;
        }
    }
}
