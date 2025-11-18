using YS.CoffeeMachine.API.Utils;
using YS.CoffeeMachine.Application.Commands.FileResourceCommands;
using YS.CoffeeMachine.Infrastructure;
using YS.Provider.OSS.Interface.Base;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.FileResourceCommandHandlers
{
    /// <summary>
    /// 文件管理
    /// </summary>
    /// <param name="_context"></param>
    public class CreateFileManageCommandHandler(CoffeeMachineDbContext _context, IOSSService _oSSService) : ICommandHandler<CreateFileManageCommand>
    {
        /// <summary>
        /// 创建文件管理
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Handle(CreateFileManageCommand request, CancellationToken cancellationToken)
        {
            var fileOpt = new FileOptionUtils(_context, _oSSService);

            // 图片操作
            string newFilePath = string.Empty;

            // 复制oss文件  并且创建饮品文件关系
            if (request.file != null)
            {
                // 自己提交的文件处理方式
                await fileOpt.FileMove(request.file, request.file.FilePath, 1, 0, true);
            }
        }
    }
}
