using FluentValidation;
using YS.CoffeeMachine.Application.Commands.TaskSchedulingInfoCommands;

namespace YS.CoffeeMachine.Platform.API.Application.Validations.TaskSchedulingInfoCommandValidators
{
    /// <summary>
    /// 创建任务调度参数验证
    /// </summary>
    public class CreateTaskSchedulingInfoCommandValidators : AbstractValidator<CreateTaskSchedulingInfoCommand>
    {
        /// <summary>
        /// 创建任务调度参数验证
        /// </summary>
        public CreateTaskSchedulingInfoCommandValidators(ILogger<CreateTaskSchedulingInfoCommand> logger)
        {
            RuleFor(command => command.name).NotEmpty().MinimumLength(1);
            RuleFor(command => command.cron).NotEmpty();
            RuleFor(command => command.isEnabled).Must(value => value == true || value == false)
    .WithMessage("The field must be selected (true or false).");
            if (logger.IsEnabled(LogLevel.Trace))
            {
                logger.LogTrace("INSTANCE CREATED - {ClassName}", GetType().Name);
            }
        }
    }
}
