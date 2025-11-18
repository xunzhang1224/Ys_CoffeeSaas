using System;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Yitter.IdGenerator;
using YS.CoffeeMachine.Application.Dtos.Files;
using YS.CoffeeMachine.Domain.AggregatesModel.Files;
using YS.CoffeeMachine.Infrastructure;
using YS.Provider.OSS.Interface.Base;

namespace YS.CoffeeMachine.API.Utils
{
    /// <summary>
    /// 文件操作相关
    /// </summary>
    /// <param name="_context"></param>
    public class FileOptionUtils(CoffeeMachineDbContext _context, IOSSService _oSSService)
    {
        /// <summary>
        /// 创建文件管理信息
        /// </summary>
        /// <returns></returns>
        public async Task<long> CreateFileManageInfoAsync(FileManageInput request)
        {
            var fileId = YitIdHelper.NextId();
            var info = new FileManage(request.FileName, request.FilePath, request.FileType, request.FileSize, request.ResorceUsage, fileId);
            await _context.FileManage.AddAsync(info);
            return fileId;
        }

        /// <summary>
        /// 创建文件关联
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="targetId"></param>
        /// <param name="targetType">
        /// 1：饮品表（BeverageInfo）
        /// 2：饮品历史版本（BeverageVersion）
        /// 3：饮品库（BeverageInfoTemplate）
        /// 4：饮品库历史版本（BeverageInfoTemplateVersion）
        /// 5：设备广告（BeverageInfoTemplateVersion）
        /// </param>
        /// <returns></returns>
        public async Task CreateFileRelation(long fileId, long targetId, int targetType)
        {
            var info = new FileRelation(fileId, targetId, targetType);
            await _context.FileRelation.AddAsync(info);
        }

        /// <summary>
        /// 文件移动
        /// </summary>
        /// <param name="request"></param>
        /// <param name="oldFileUrl"></param>
        /// <param name="targetType">
        /// 1：饮品表（BeverageInfo）
        /// 2：饮品历史版本（BeverageVersion）
        /// 3：饮品库（BeverageInfoTemplate）
        /// 4：饮品库历史版本（BeverageInfoTemplateVersion）
        /// </param>
        /// <returns></returns>
        public async Task<Dictionary<long, string>> FileMove(FileManageInput input, string oldFileUrl, int targetType, long targetId, bool? passRelation = false)
        {
            // 获取文件名
            string fileName = Path.GetFileName(new Uri(oldFileUrl).AbsolutePath);
            var dateTime = DateTime.Now.ToString("yyyyMMdd");

            // 获取 Uri 对象的 AbsolutePath，并去掉开头的 '/'
            var objectKey = new Uri(oldFileUrl).AbsolutePath.TrimStart('/');
            string newObjecKey = input.ResorceUsage + "/" + dateTime + "/" + fileName;

            // 获取地址
            var baseUrl = new Uri(oldFileUrl).GetLeftPart(UriPartial.Authority);

            input.FilePath = baseUrl + "/" + newObjecKey;

            var fileId = await CreateFileManageInfoAsync(input);

            if (passRelation == null || passRelation == false)
            {
                // 创建文件关联
                await CreateFileRelation(fileId, targetId, targetType);
            }

            // 复制文件（最后复制） var copyRequest = new CopyObjectRequest("your-bucket", "source/path/file.jpg", "your-bucket", "target/path/file.jpg");
            await _oSSService.CopyObjectAsync("ourvend-kfj", objectKey, "ourvend-kfj", newObjecKey);

            var result = new Dictionary<long, string>
            {
                {fileId, input.FilePath}
            };
            return result;
        }
    }
}
