using FluentValidation;
using YS.CoffeeMachine.Application.Commands.SettingsCommands.InterfaceStylesCommands;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.Validations.SettingsCommandValidators.InterfaceStylesCommands
{
    /// <summary>
    /// 更新界面样式验证
    /// </summary>
    public class UpdateInterfaceStylesCommandValidators : AbstractValidator<UpdateInterfaceStylesCommand>
    {
        /// <summary>
        /// 更新界面样式验证
        /// </summary>
        public UpdateInterfaceStylesCommandValidators()
        {
            RuleFor(r => r.name).NotEmpty().WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0001)], "name"));
            RuleFor(r => r.code).NotEmpty().WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0001)], "code"));
            RuleFor(r => r.preview).NotEmpty().WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0001)], "preview"));
        }
    }
}
