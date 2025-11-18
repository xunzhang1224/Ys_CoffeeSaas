using FreeRedis;
using Magicodes.ExporterAndImporter.Core;
using MediatR;
using System.Threading.Channels;
using System.Threading.Tasks;
using Yitter.IdGenerator;
using YS.CoffeeMachine.Application.Commands.BasicCommands.DocmentCommands;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Provider.Dto.Docment;
using YS.Util.Core;

namespace YS.CoffeeMachine.API.Extensions.IExecl
{
    /// <summary>
    /// 导出服务
    /// </summary>
    /// <param name="_redisClent"></param>
    /// <param name="_user"></param>
    /// <param name="_exporter"></param>
    public class ExportService(IRedisClient _redisClent,
        UserHttpContext _user,
        IMediator _mediator,
        IExporter _exporter)
    {
        /// <summary>
        /// 导出服务
        /// </summary>
        public async Task ExportAsByteArray<T>(List<T> data, DocmentTypeEnum type, Channel<FileExcelUploadTask> channel) where T : class, new()
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
            await channel.Writer.WriteAsync(uploadTask);
            await _mediator.Send(new CreateFileNewCenterCommand(uploadTask.FileName, (DocmentTypeEnum)uploadTask.FileCenterType, uploadTask.TenantId, uploadTask.Code));
        }
    }
}
