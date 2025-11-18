using Aliyun.OSS;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using Yitter.IdGenerator;
using YS.CoffeeMachine.API.Utils;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoCommands;
using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
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
    /// 添加饮品
    /// </summary>
    /// <param name="context"></param>
    public class CreateBeverageInfoCommandHandler(CoffeeMachineDbContext context, IOSSService _oSSService) : ICommandHandler<CreateBeverageInfoCommand, string>
    {
        /// <summary>
        /// 添加饮品
        /// </summary>
        public async Task<string> Handle(CreateBeverageInfoCommand request, CancellationToken cancellationToken)
        {
            var fileOpt = new FileOptionUtils(context, _oSSService);
            long id = YitIdHelper.NextId();

            // 图片操作

            string newFilePath = string.Empty;
            long fileId = 0;
            // 复制oss文件  并且创建饮品文件关系
            if (request.file != null)
            {
                // 自己提交的文件处理方式
                var fileDic = await fileOpt.FileMove(request.file, request.beverageIcon, 1, id);
                fileId = fileDic.Keys.First();
                newFilePath = fileDic[fileId];
            }
            else
            {
                // 如果是用的资源库的图片
                fileId = await context.FileManage.AsQueryable().Where(w => w.FilePath == request.beverageIcon).Select(s => s.Id).FirstOrDefaultAsync();
                await fileOpt.CreateFileRelation(fileId, id, 1);
            }

            newFilePath = string.IsNullOrWhiteSpace(newFilePath) ? request.beverageIcon : newFilePath;

            var info = new BeverageInfo(request.deviceInfoId, request.name, newFilePath, request.temperature, request.remarks, request.productionForecast, request.forecastQuantity
                , request.displayStatus, false, id, request.sellStradgy, request.price, request.discountedPrice, productCategoryIds: request.productCategoryIds);
            if (!string.IsNullOrWhiteSpace(request.code))
            {
                var hasCode = await context.BeverageInfo.FirstOrDefaultAsync(w => w.DeviceId == info.DeviceId && w.Code == request.code);
                if (hasCode != null)
                {
                    throw ExceptionHelper.AppFriendly(string.Format(L.Text[nameof(ErrorCodeEnum.D0054)], request.code));
                }
                info.AddCodeNotId(request.code);
            }
            else
            {
                info.AddCodeNotId(id.ToString());
            }
            // Code不显示
            info.UpdateCodeIsShow(true);

            var formulaInfos = new List<FormulaInfo>();
            //var hasDefaluMaterial = false;
            if (request.formulaInfoDtos == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0055)]);
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

            // 添加历史版本信息
            long beverageVersionId = YitIdHelper.NextId();

            if (fileId != 0 && fileId != null)
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
            info.InsertBeverageVersions(BeverageVersionTypeEnum.Insert, jsonData, beverageVersionId);

            await context.AddAsync(info);

            //return info.Id;
            return info.Code;
            //return true;
        }
    }
}