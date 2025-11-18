using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.FileResourceCommands;
using YS.CoffeeMachine.Infrastructure;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.FileResourceCommandHandlers
{
    /// <summary>
    /// 文件管理
    /// </summary>
    /// <param name="_context"></param>
    public class UpdateFileManageCommandHandler(CoffeeMachineDbContext _context) : ICommandHandler<UpdateFileManageCommand>
    {
        /// <summary>
        /// 修改文件管理
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Handle(UpdateFileManageCommand request, CancellationToken cancellationToken)
        {
            var info = await _context.FileManage.AsQueryable().Where(a => a.Id == request.id).FirstOrDefaultAsync();
            info.Update(request.fileName, request.resorceUsage);
        }
    }
}
