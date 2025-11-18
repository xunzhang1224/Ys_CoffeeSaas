using FluentValidation;
using YS.CoffeeMachine.Application.Commands.ApplicationCommands.RolesCommands;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.Validations.ApplicationCommandValidators
{
    /// <summary>
    /// 创建角色验证
    /// </summary>
    public class CreateApplicationRoleCommandValidators : AbstractValidator<CreateApplicationRoleCommand>
    {
        /// <summary>
        /// 创建角色验证
        /// </summary>
        public CreateApplicationRoleCommandValidators(ILogger<CreateApplicationRoleCommand> logger)
        {
            RuleFor(r => r.name).NotEmpty().WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0001)], "Name"));
            //RuleFor(r => r.code).NotEmpty();
            RuleFor(r => r.roleStatus).IsInEnum().WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0004)], "RoleStatus"));
            RuleFor(r => r.sort).Must(x => true).WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0008)]));
            //RuleFor(r => r.sysMenuType).IsInEnum().WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0004)], "sysMenuType"));
        }
    }
}
