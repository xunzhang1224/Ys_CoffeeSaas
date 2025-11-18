using FluentValidation;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceInfoCommands;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.Validations.DevicesCommandValidators
{
    /// <summary>
    /// 编辑设备验证
    /// </summary>
    public class UpdateDeviceInfoCommandValidators : AbstractValidator<UpdateDeviceInfoCommand>
    {
        /// <summary>
        /// 编辑设备验证
        /// </summary>
        public UpdateDeviceInfoCommandValidators(ILogger<UpdateDeviceInfoCommand> logger)
        {
            RuleFor(r => r.id).NotEmpty().WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0001)], "id"));
            RuleFor(r => r.devicePositionVo.CountryRegionIds).NotEmpty().WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0001)], "countryRegionIds"));
            RuleFor(r => r.name).NotEmpty().WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0001)], "name"));
            RuleFor(r => r.devicePositionVo.CountryId).NotEmpty().WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0001)], "countryId"));
            //RuleFor(r => r.equipmentNumber).NotEmpty().WithMessage(string.Format(L.Text[nameof(ErrorCodeEnum.C0001)], "equipmentNumber"));
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation($"编辑设备验证失败 - {GetType().Name}");
            }
        }
    }
}
