using FluentValidation;
using YS.CoffeeMachine.Application.Commands.ApplicationCommands.MenuCommands;
using YS.CoffeeMachine.Application.Commands.ApplicationCommands.RolesCommands;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.Validations.ApplicationCommandValidators
{
    /// <summary>
    /// 创建菜单
    /// </summary>
    public class CreateApplicationMenuCommandValidators : AbstractValidator<CreateApplicationMenuCommand>
    {
        /// <summary>
        /// 创建菜单
        /// </summary>
        public CreateApplicationMenuCommandValidators(ILogger<CreateApplicationRoleCommand> logger)
        {
            RuleFor(r => r.sysMenuType).IsInEnum().WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0004)], "SysMenuType"));//.WithMessage("SysMenuType 必须是有效的枚举值");
            RuleFor(r => r.menuType).IsInEnum().WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0004)], "SubType"));
            RuleFor(r => r.title).NotEmpty().WithMessage(L.Text[nameof(ErrorCodeEnum.C0001)]);
        }
    }
}
