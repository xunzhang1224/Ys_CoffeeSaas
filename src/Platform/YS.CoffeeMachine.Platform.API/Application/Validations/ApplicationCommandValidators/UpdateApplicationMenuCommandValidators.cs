using FluentValidation;
using YS.CoffeeMachine.Application.Commands.ApplicationCommands.MenuCommands;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.Validations.ApplicationCommandValidators
{
    /// <summary>
    /// 更新菜单验证
    /// </summary>
    public class UpdateApplicationMenuCommandValidators : AbstractValidator<UpdateApplicationMenuCommand>
    {
        /// <summary>
        /// 更新菜单验证
        /// </summary>
        public UpdateApplicationMenuCommandValidators(ILogger<UpdateApplicationMenuCommand> logger)
        {
            RuleFor(r => r.sysMenuType).IsInEnum().WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0004)], "sysMenuType"));
            RuleFor(r => r.menuType).IsInEnum().WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0004)], "menuType"));
            RuleFor(r => r.title).NotEmpty().WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0001)], "title"));
        }
    }
}