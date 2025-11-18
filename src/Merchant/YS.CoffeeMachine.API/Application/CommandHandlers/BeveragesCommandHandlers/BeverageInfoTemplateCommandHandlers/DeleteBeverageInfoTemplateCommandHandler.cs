using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoTemplateCommands;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.BeveragesCommandHandlers.BeverageInfoTemplateCommandHandlers
{
    /// <summary>
    /// 批量删除饮品模板
    /// </summary>
    /// <param name="context"></param>
    public class DeleteBeverageInfoTemplateCommandHandler(CoffeeMachineDbContext context) : ICommandHandler<DeleteBeverageInfoTemplateCommand, bool>
    {
        /// <summary>
        /// 批量删除饮品模板
        /// </summary>
        public async Task<bool> Handle(DeleteBeverageInfoTemplateCommand request, CancellationToken cancellationToken)
        {
            if (request.ids.Count == 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0011)]);
            var list = context.BeverageInfoTemplate.Where(w => request.ids.Contains(w.Id)).ToList();
            list.ForEach(e => { e.IsDelete = true; });

            #region 删除饮品库及饮品库历史版本 图片关联信息
            var beverageTemplateVsersionIds = await context.BeverageTemplateVersion.AsQueryable().Where(w => request.ids.Contains(w.BeverageInfoTemplateId) && w.VersionType != BeverageVersionTypeEnum.Collection).Select(s => s.Id).ToListAsync();
            var allIds = request.ids.Concat(beverageTemplateVsersionIds).ToList();
            context.FileRelation.RemoveRange(context.FileRelation.Where(w => allIds.Contains(w.TargetId)));
            #endregion

            return true;
        }
    }
}
