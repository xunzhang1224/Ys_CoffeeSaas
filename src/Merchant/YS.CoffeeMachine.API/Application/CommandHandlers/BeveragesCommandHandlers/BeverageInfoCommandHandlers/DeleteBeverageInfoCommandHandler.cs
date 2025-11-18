using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoCommands;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.BeveragesCommandHandlers.BeverageInfoCommandHandlers
{
    /// <summary>
    /// 批量删除饮品
    /// </summary>
    /// <param name="context"></param>
    public class DeleteBeverageInfoCommandHandler(CoffeeMachineDbContext context) : ICommandHandler<DeleteBeverageInfoCommand, bool>
    {
        /// <summary>
        /// 批量删除饮品
        /// </summary>
        public async Task<bool> Handle(DeleteBeverageInfoCommand request, CancellationToken cancellationToken)
        {
            if (request.ids.Count() == 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0013)]);
            var info = await context.BeverageInfo.Where(w => request.ids.Contains(w.Id)).ToListAsync();
            if (info == null || info.Count == 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            foreach (var item in info)
            {
                if (item.IsDefault)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0057)]);
                item.IsDelete = true;
                item.AddCode(item.Code);
            }

            #region 删除文件关联

            // 根据饮品id查询 饮品历史对应ids
            var beverageVersionIds = await context.BeverageVersion.AsQueryable().Where(w => request.ids.Contains(w.BeverageInfoId) && w.VersionType != BeverageVersionTypeEnum.Collection).Select(s => s.Id).ToListAsync();

            // 合并目标ids
            var mergedList = request.ids.Concat(beverageVersionIds).ToList();

            context.FileRelation.RemoveRange(context.FileRelation.Where(w => mergedList.Contains(w.TargetId)));
            #endregion

            return true;
        }
    }
}
