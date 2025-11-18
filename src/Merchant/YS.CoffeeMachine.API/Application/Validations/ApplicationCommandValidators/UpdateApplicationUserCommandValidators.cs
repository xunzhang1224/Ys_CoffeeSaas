using FluentValidation;
using YS.CoffeeMachine.Application.Commands.ApplicationCommands.UsersCommands;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.Validations.ApplicationCommandValidators
{
    /// <summary>
    /// 修改用户验证
    /// </summary>
    public class UpdateApplicationUserCommandValidators : AbstractValidator<UpdateApplicationUserCommand>
    {
        /// <summary>
        /// 修改用户验证
        /// </summary>
        /// <param name="logger"></param>
        public UpdateApplicationUserCommandValidators(ILogger<UpdateApplicationUserCommand> logger)
        {
            RuleFor(r => r.id).NotEmpty().WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0001)], "id"));
            RuleFor(r => r.account).NotEmpty();
            RuleFor(r => r.nickName).NotEmpty();
            //RuleFor(r => r.phone).NotEmpty();
            RuleFor(r => r.email).NotEmpty().WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0001)], "email")).EmailAddress().WithMessage(L.Text[nameof(ErrorCodeEnum.C0009)]);
            RuleFor(r => r.status).IsInEnum().WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0010)], "status"));
            //RuleFor(r => r.accountType).IsInEnum().WithMessage("AccountType 必须是有效的枚举值");
            //RuleFor(r => r.sysMenuType).IsInEnum().WithMessage("SysMenuType 必须是有效的枚举值");
            if (logger.IsEnabled(LogLevel.Trace))
            {
                logger.LogTrace("INSTANCE CREATED - {ClassName}", GetType().Name);
            }
        }
    }
}
