using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.PlatformCommands.PaymentCommands;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.PaymentCommandHandlers
{
    /// <summary>
    /// 更新平台支付配置
    /// </summary>
    /// <param name="context">上下文</param>
    public class UpdatePaymentConfigCommandHander(CoffeeMachinePlatformDbContext context) : ICommandHandler<UpdatePaymentConfigCommand, bool>
    {
        /// <summary>
        /// 更新平台支付配置
        /// </summary>
        public async Task<bool> Handle(UpdatePaymentConfigCommand request, CancellationToken cancellationToken)
        {
            var info = await context.P_PaymentConfig.AsQueryable()
                .Where(a => a.Id == request.id).FirstOrDefaultAsync();
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

            info.Update(request.countrys, request.pictureUrl, request.enable);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
