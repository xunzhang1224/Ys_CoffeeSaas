using FreeRedis;
using Magicodes.ExporterAndImporter.Core;
using MediatR;
using System.Threading.Channels;
using Yitter.IdGenerator;
using YS.CoffeeMachine.Application.PlatformCommands.BasicCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics.Docment;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Provider.Dto.Docment;
using YS.Util.Core;

namespace YS.CoffeeMachine.Platform.API.Extensions.IExecl
{
    /// <summary>
    /// 导出服务
    /// </summary>
    /// <param name="_redisClent"></param>
    /// <param name="_exporter"></param>
    public class ExportService(IRedisClient _redisClent,
        UserHttpContext _user,
        IMediator _mediator,
        CoffeeMachinePlatformDbContext _db,
        IExporter _exporter)
    {
        /// <summary>
        /// 导出
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="langCode"></param>
        /// <param name="type"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public async Task ExportAsByteArray<T>(List<T> data, string langCode, DocmentTypeEnum type, Channel<FileExcelUploadTask> channel) where T : class, new()
        {
            var bytes = await _exporter.ExportAsByteArray(data);
            string fileCenterDesc = type.GetDescription();
            string datetime = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var fileNameValue = $"{fileCenterDesc}{datetime}.xlsx";

            var uploadTask = new FileExcelUploadTask
            {
                FileName = fileNameValue,
                DataItems = bytes,
                FileCenterType = (int)type,
                TenantId = _user.TenantId,
                Code = YitIdHelper.NextId().ToString(),
            };
            await _mediator.Send(new CreateFileNewCenterCommand(uploadTask.FileName, (DocmentTypeEnum)uploadTask.FileCenterType, uploadTask.TenantId, uploadTask.Code));
            //var file = new FileCenter(uploadTask.FileName, uploadTask.Code, type, uploadTask.TenantId, SysMenuTypeEnum.Platform);
            //await _db.AddAsync(file);
            //await _db.SaveChangesAsync();
            await channel.Writer.WriteAsync(uploadTask);
        }
    }
}
