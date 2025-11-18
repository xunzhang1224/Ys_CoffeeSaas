using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.FileResourceCommands;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;
using static NPOI.HSSF.UserModel.HeaderFooter;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.FileResourceCommandHandlers
{
    /// <summary>
    /// 删除文件管理
    /// </summary>
    /// <param name="_context"></param>
    public class DeleteFileManageCommandHandler(CoffeeMachineDbContext _context) : ICommandHandler<DeleteFileManageCommand>
    {
        /// <summary>
        /// 删除文件管理
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Handle(DeleteFileManageCommand request, CancellationToken cancellationToken)
        {
            var relation = await _context.FileRelation
              .Where(f => request.ids.Contains(f.FileId))
            .ToListAsync();

            if (relation.Count > 0)
            {
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0087)]);
            }

            var fileManageInfos = await _context.FileManage.Where(w => request.ids.Contains(w.Id)).ToListAsync();
            foreach (var fileManageInfo in fileManageInfos)
            {
                fileManageInfo.IsDelete = true;
            }
        }
    }
}
