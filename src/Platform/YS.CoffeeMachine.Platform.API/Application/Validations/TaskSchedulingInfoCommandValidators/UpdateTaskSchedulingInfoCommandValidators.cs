using FluentValidation;
using YS.CoffeeMachine.Application.Commands.TaskSchedulingInfoCommands;

namespace YS.CoffeeMachine.Platform.API.Application.Validations.TaskSchedulingInfoCommandValidators
{
    /// <summary>
    /// 编辑任务调度参数验证
    /// </summary>
    public class UpdateTaskSchedulingInfoCommandValidators : AbstractValidator<UpdateTaskSchedulingInfoCommand>
    {
        /// <summary>
        /// 编辑任务调度参数验证
        /// </summary>
        public UpdateTaskSchedulingInfoCommandValidators(ILogger<UpdateTaskSchedulingInfoCommand> logger)
        {
            RuleFor(command => command.id).NotEmpty();
            RuleFor(command => command.name).NotEmpty().MinimumLength(1);
            RuleFor(command => command.cron).NotEmpty();
            RuleFor(command => command.isEnabled).Must(value => value == true || value == false)
    .WithMessage("The field must be selected (true or false)."); ;
            if (logger.IsEnabled(LogLevel.Trace))
            {
                logger.LogTrace("INSTANCE CREATED - {ClassName}", GetType().Name);
            }
        }
    }
}
