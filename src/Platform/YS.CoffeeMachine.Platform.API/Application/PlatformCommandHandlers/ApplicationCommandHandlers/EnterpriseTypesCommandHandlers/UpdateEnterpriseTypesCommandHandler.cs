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
    /// 更新企业类型
    /// </summary>
    /// <param name="context"></param>
    public class UpdateEnterpriseTypesCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<UpdateEnterpriseTypesCommand, bool>
    {
        /// <summary>
        /// 更新企业类型
        /// </summary>
        public async Task<bool> Handle(UpdateEnterpriseTypesCommand request, CancellationToken cancellationToken)
        {
            var info = await context.EnterpriseTypes.FirstAsync(w => w.Id == request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            if (info.IsDefault)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0081)]);
            // 验证企业类型名称是否存在 && x.Astrict == request.astrict
            var isExist = await context.EnterpriseTypes.AnyAsync(x => x.Name == request.name && x.Id != request.id);
            if (isExist)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0080)]);
            info.Update(request.name, request.astrict);
            return true;
        }
    }
}
