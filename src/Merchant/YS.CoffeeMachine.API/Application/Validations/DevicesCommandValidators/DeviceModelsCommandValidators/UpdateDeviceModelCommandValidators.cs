using FluentValidation;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceModelCommands;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.Validations.DevicesCommandValidators.DeviceModelsCommandValidators
{
    /// <summary>
    /// 编辑设备型号验证
    /// </summary>
    public class UpdateDeviceModelCommandValidators : AbstractValidator<UpdateDeviceModelCommand>
    {
        /// <summary>
        /// 编辑设备型号验证
        /// </summary>
        public UpdateDeviceModelCommandValidators()
        {
            RuleFor(r => r.name).NotEmpty().WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0001)], "name"));
            RuleFor(r => r.maxCassetteCount).GreaterThanOrEqualTo(0).NotEmpty().WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0001)], "maxCassetteCount"));
            RuleFor(r => r.remark).NotEmpty().WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0001)], "remark"));
        }
    }
}
