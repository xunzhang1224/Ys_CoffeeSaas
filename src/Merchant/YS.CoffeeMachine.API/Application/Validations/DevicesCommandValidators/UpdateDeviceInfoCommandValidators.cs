using FluentValidation;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.DeviceInfoCommands;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.Validations.DevicesCommandValidators
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
            RuleFor(r => r.id).NotEmpty();
            //RuleFor(r => r.countryRegionIds).NotEmpty();
            RuleFor(r => r.name).NotEmpty();
            //RuleFor(r => r.countryId).NotEmpty();
            //RuleFor(r => r.deviceModelId).NotEmpty();
            RuleFor(r => r.versionNumber).NotEmpty();
            RuleFor(r => r.equipmentNumber).NotEmpty();
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation($"编辑设备验证失败 - {GetType().Name}");
            }
        }
    }
}
