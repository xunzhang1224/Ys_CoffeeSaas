using FluentValidation;
using YS.CoffeeMachine.Application.Commands.ApplicationCommands.RolesCommands;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.Validations.ApplicationCommandValidators
{
    /// <summary>
    /// 更新角色验证
    /// </summary>
    public class UpdateApplicationRoleCommandValidators : AbstractValidator<UpdateApplicationRoleCommand>
    {
        /// <summary>
        /// 更新角色验证
        /// </summary>
        public UpdateApplicationRoleCommandValidators(ILogger<UpdateApplicationRoleCommand> logger)
        {
            RuleFor(r => r.name).NotEmpty();
            RuleFor(r => r.roleStatus).IsInEnum().WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0004)], "RoleStatus"));
            RuleFor(r => r.sort).Must(x => true).WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0005)], "sort"));
            RuleFor(r => r.sysMenuType).IsInEnum().WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0004)], "sysMenuType"));
        }
    }
}
