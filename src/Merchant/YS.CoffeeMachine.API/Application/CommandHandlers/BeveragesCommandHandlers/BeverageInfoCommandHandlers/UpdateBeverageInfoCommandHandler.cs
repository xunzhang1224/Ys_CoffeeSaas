using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text.Json;
using Yitter.IdGenerator;
using YS.CoffeeMachine.API.Utils;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoCommands;
using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YS.CoffeeMachine.Domain.AggregatesModel.Files;
using YS.CoffeeMachine.Domain.IRepositories.BeveragesRepositorys;
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
    /// 修改饮品信息
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="context"></param>
    public class UpdateBeverageInfoCommandHandler(IBeverageInfoRepository repository, CoffeeMachineDbContext context, IOSSService _oSSService) : ICommandHandler<UpdateBeverageInfoCommand, bool>
    {
        /// <summary>
        /// 修改饮品信息
        /// </summary>
        public async Task<bool> Handle(UpdateBeverageInfoCommand request, CancellationToken cancellationToken)
        {
            var info = await context.BeverageInfo.Include(i => i.FormulaInfos).FirstAsync(w => w.Id == request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            var curCode = request.code;

            // 编辑去掉默认code
            //if (string.IsNullOrWhiteSpace(curCode))
            //    curCode = info.TransId.ToString();

            // 编辑饮品时，饮品Code不能与当前设备其他饮品重复  为空除外
            if (!string.IsNullOrWhiteSpace(curCode))
            {
                var hasCode = await context.BeverageInfo.FirstOrDefaultAsync(w => w.DeviceId == info.DeviceId && w.Id != info.Id && w.Code == curCode);
                if (hasCode != null)
                {
                    throw ExceptionHelper.AppFriendly(string.Format(L.Text[nameof(ErrorCodeEnum.D0054)], request.code));
                }
                info.AddCodeNotId(curCode);
                info.UpdateCodeIsShow(true);
            }
            else
            {
                info.AddCodeNotId(info.Id.ToString());
                info.UpdateCodeIsShow(true);
            }

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
            {
                if (fileRelationInfo != null)
                    fileRelationInfo.UpdateFileId(fileId);
                else
                {
                    await context.FileRelation.AddAsync(new FileRelation(fileId, info.Id, 1));
                }
            }

            newFilePath = string.IsNullOrWhiteSpace(newFilePath) ? request.beverageIcon : newFilePath;
            #endregion

            info.Update(request.name, newFilePath, request.temperature, request.remarks, request.productionForecast, request.forecastQuantity, request.displayStatus, request.sellStradgy, productCategoryIds: request.productCategoryIds);
            info.UpdatePriceInfo(request.price, request.discountedPrice);

            var formulaInfos = new List<FormulaInfo>();
            //var hasDefaluMaterial = false;
            foreach (var item in request.formulaInfoDtos)
            {
                if (!FormulaSpecsConverter.ValidateSpecsJson(item.FormulaType, item.Specs.ToString()))
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0056)]);
                formulaInfos.Add(new FormulaInfo(info.Id, item.MaterialBoxId, item.MaterialBoxName, item.Sort, item.FormulaType, item.Specs));
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
            await fileOpt.CreateFileRelation(fileId, beverageVersionId, 2);
            info.InsertBeverageVersions(BeverageVersionTypeEnum.Edit, jsonData, beverageVersionId);

            var res = await repository.UpdateAsync(info);
            return res != null;
        }
    }
}
