using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.EnterpriseTypesCommands;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.ApplicationCommandHandlers.EnterpriseTypesCommandHandlers
{
    /// <summary>
    /// 更新企业类型状态命令处理程序
    /// </summary>
    /// <param name="context"></param>
    public class UpdateEnterpriseTypeStatusCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<UpdateEnterpriseTypeStatusCommand, bool>
    {
        /// <summary>
        /// 更新企业类型状态命令处理程序
        /// </summary>
        public async Task<bool> Handle(UpdateEnterpriseTypeStatusCommand request, CancellationToken cancellationToken)
        {
            var info = await context.EnterpriseTypes.FirstAsync(w => w.Id == request.Id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            info.UpdateStatus(request.Status);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
