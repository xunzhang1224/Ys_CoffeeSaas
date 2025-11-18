using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.ApplicationCommands.UsersCommands;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.ApplicationCommandHandlers.UsersCommandHandlers
{
    /// <summary>
    /// 手机号验证通过
    /// </summary>
    /// <param name="context"></param>
    public class PhoneVerificationPassedCommandHandler(CoffeeMachineDbContext context) : ICommandHandler<PhoneVerificationPassedCommand, bool>
    {
        /// <summary>
        /// 手机号验证通过
        /// </summary>
        public async Task<bool> Handle(PhoneVerificationPassedCommand request, CancellationToken cancellationToken)
        {
            var info = await context.ApplicationUser.AsNoTracking().FirstAsync(w => w.Id == request.userId);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            info.IsVerifyPhone();
            var res = context.Update(info);
            return res != null;
        }
    }
}
