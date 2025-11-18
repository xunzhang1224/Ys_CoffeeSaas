using Magicodes.ExporterAndImporter.Core.Extension;
using MediatR;
using System.Threading.Channels;
using YS.CoffeeMachine.Application.PlatformCommands.BasicCommands;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Provider.Dto.Docment;
using YS.Provider.OSS.Interface.Base;
using YS.Util.Core;
namespace YS.CoffeeMachine.Platform.API.Extensions.IExecl
{
    /// <summary>
    /// FileUploadBackgroundService
    /// </summary>
    /// <param name="scopeFactory"></param>
    /// <param name="_channel"></param>
    /// <param name="_logger"></param>
    /// <param name="_iOSSService"></param>
    public class FileUploadBackgroundService(IServiceScopeFactory scopeFactory,
        Channel<FileExcelUploadTask> _channel,
        ILogger<FileUploadBackgroundService> _logger,

        IOSSService _iOSSService) : BackgroundService
    {
        /// <summary>
        /// ExecuteAsync
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var _serviceScope = scopeFactory.CreateScope();
            var _mediator = _serviceScope.ServiceProvider.GetRequiredService<IMediator>();
            var fileName = string.Empty;

            await foreach (var task in _channel.Reader.ReadAllAsync(stoppingToken))
            {
                try
                {
                    var createTime = DateTime.Now; // 用户没绑定时区，取当前系统时间
                    var result = await UploadToOssAsync(task.DataItems, task.FileName, (DocmentTypeEnum)task.FileCenterType, createTime);
                    await _mediator.Send(new UpdateFileCommand(task.Code, result.isSucess? FileStateEnum.Success: FileStateEnum.Fail,null, result.url));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "导出文件时出错");
                }
            }
        }

        private async Task<(string fileName, string url, bool isSucess)> UploadToOssAsync(
            byte[] bytes,
            string fileName,
            DocmentTypeEnum fileCenterType,
            DateTime dateTime)
        {
            string fileCenter = OssDescribeConst.DefaultFileCenter;
            string objectName;
            // Generate object name and file name if necessary
            (objectName, fileName) = GenerateObjectNameAndFileName(fileName, fileCenterType, fileCenter, dateTime);

            bool isSucess;
            var url = string.Empty;
            using (var stream = new MemoryStream(bytes))
            {
                isSucess = await _iOSSService.PutObjectAsync(OssDescribeConst.DefaultBucketName, objectName, stream);
            }
            if (isSucess)
            {
                url = await _iOSSService.PresignedGetObjectAsync(OssDescribeConst.DefaultBucketName, objectName, 900);
            }
            return (fileName, url, isSucess);
        }

        private (string objectName, string fileName) GenerateObjectNameAndFileName(
            string fileName,
            DocmentTypeEnum fileCenterType,
            string fileCenter,
            DateTime datetime)
        {
            string datetimeStr = datetime.ToString("yyyyMMddHHmmss");
            string dateStr = datetime.ToString("yyyyMMdd");

            if (!string.IsNullOrEmpty(fileName))
            {
                fileName.CheckExcelFileName();
                string objectName = fileCenter + "/" + dateStr + "/" + fileName;
                return (objectName, fileName);
            }
            else
            {
                string fileCenterDesc = fileCenterType.GetDescription();
                fileName = $"{fileCenterDesc}{datetimeStr}.xlsx";
                string objectName = fileCenter + "/" + fileName;
                return (objectName, fileName);
            }
        }
    }
}
