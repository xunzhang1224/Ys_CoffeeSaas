using FluentValidation;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceModelCommands;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.Validations.DevicesCommandValidators.DeviceModelsCommandValidators
{
    /// <summary>
    /// 创建设备型号验证
    /// </summary>
    public class CreateDeviceModelCommandValidators : AbstractValidator<CreateDeviceModelCommand>
    {
        /// <summary>
        /// 创建设备型号验证
        /// </summary>
        public CreateDeviceModelCommandValidators()
        {
            RuleFor(r => r.name).NotEmpty().WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0001)], "name"));
            RuleFor(r => r.maxCassetteCount).GreaterThanOrEqualTo(0).NotEmpty().WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0001)], "maxCassetteCount"));
            RuleFor(r => r.remark).NotEmpty().WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0001)], "remark"));
        }
    }
}
