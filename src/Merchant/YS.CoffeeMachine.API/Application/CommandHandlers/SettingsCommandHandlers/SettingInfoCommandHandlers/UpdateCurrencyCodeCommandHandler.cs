using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.SettingsCommands.SettingInfoCommands;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.SettingsCommandHandlers.SettingInfoCommandHandlers
{
    /// <summary>
    /// 更新货币符号
    /// </summary>
    /// <param name="context"></param>
    public class UpdateCurrencyCodeCommandHandler(CoffeeMachineDbContext context) : ICommandHandler<UpdateCurrencyCodeCommand, bool>
    {
        /// <summary>
        /// 更新货币符号
        /// </summary>
        public async Task<bool> Handle(UpdateCurrencyCodeCommand request, CancellationToken cancellationToken)
        {
            var info = await context.SettingInfo.FirstOrDefaultAsync(w => w.Id == request.settingInfoId);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            info.SetCurrencyCode(request.currencyCode);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
