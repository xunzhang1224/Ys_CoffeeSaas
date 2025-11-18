using FluentValidation;
using YS.CoffeeMachine.Application.Commands.ApplicationCommands.UsersCommands;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.Validations.ApplicationCommandValidators
{
    /// <summary>
    /// 创建用户验证
    /// </summary>
    public class CreateApplicationUserCommandValidators : AbstractValidator<CreateApplicationUserCommand>
    {
        /// <summary>
        /// 创建用户验证
        /// </summary>
        public CreateApplicationUserCommandValidators(ILogger<CreateApplicationUserCommand> logger)
        {
            RuleFor(r => r.enterpriseId).NotEmpty().WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0001)], "enterpriseId"));
            RuleFor(r => r.account).NotEmpty().WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0001)], "account"));
            RuleFor(r => r.nickName).NotEmpty().WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0001)], "nickName"));
            //RuleFor(r => r.phone).NotEmpty().WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0001)], "phone"));
            RuleFor(r => r.email).NotEmpty().WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0001)], "email")).EmailAddress().WithMessage(L.Text[nameof(ErrorCodeEnum.C0009)]);
            //RuleFor(r => r.accountType).IsInEnum().WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0004)], "accountType"));
        }
    }
}
