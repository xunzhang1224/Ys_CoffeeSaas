using YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoTemplateCommands;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.BeveragesCommandHandlers.BeverageInfoTemplateCommandHandlers
{
    /// <summary>
    /// 批量删除饮品模板
    /// </summary>
    /// <param name="context"></param>
    public class DeleteBeverageInfoTemplateCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<DeleteBeverageInfoTemplateCommand, bool>
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
            return true;
        }
    }
}
