using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.PaymentCommands;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.PaymentCommandHandlers
{
    /// <summary>
    /// 更新支付配置备注
    /// </summary>
    /// <param name="context"></param>
    public class UpdatePaymentConfigRemarkCommandHandler(CoffeeMachineDbContext context) : ICommandHandler<UpdatePaymentConfigRemarkCommand>
    {
        /// <summary>
        /// 更新支付配置备注
        /// </summary>
        public async Task Handle(UpdatePaymentConfigRemarkCommand request, CancellationToken cancellationToken)
        {
            var info = await context.PaymentConfig.AsQueryable().Where(a => a.Id == request.id).FirstOrDefaultAsync();
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

            info.UpdateRemark(request.remark);
        }
    }
}
