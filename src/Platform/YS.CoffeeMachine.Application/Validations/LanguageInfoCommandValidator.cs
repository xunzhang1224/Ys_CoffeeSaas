namespace YS.CoffeeMachine.Application.Validations
{
    using FluentValidation;
    using Microsoft.Extensions.Logging;

    using YS.CoffeeMachine.Application.PlatformCommands.BasicCommands.LanguageCommands;

    /// <summary>
    /// 创建语言信息命令的验证器
    /// </summary>
    public class CreateLanguageInfoCommandValidator : AbstractValidator<CreateLanguageInfoCommand>
    {
        /// <summary>
        /// 构造函数，定义创建命令的验证规则
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public CreateLanguageInfoCommandValidator(ILogger<CreateLanguageInfoCommand> logger)
        {
            // code 字段必须非空
            RuleFor(command => command.code).NotEmpty();

            if (logger.IsEnabled(LogLevel.Trace))
            {
                logger.LogTrace("INSTANCE CREATED - {ClassName}", GetType().Name);
            }
        }
    }

    /// <summary>
    /// 删除语言信息命令的验证器
    /// </summary>
    public class DeleteLanguageInfoCommandValidator : AbstractValidator<DeleteLanguageInfoCommand>
    {
        /// <summary>
        /// 构造函数，定义删除命令的验证规则
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public DeleteLanguageInfoCommandValidator(ILogger<DeleteLanguageInfoCommand> logger)
        {
            // code 字段必须非空
            RuleFor(command => command.code).NotEmpty();
        }
    }

    /// <summary>
    /// 更新语言信息命令的验证器
    /// </summary>
    public class UpdateLanguageInfoCommandValidator : AbstractValidator<UpdateLanguageInfoCommand>
    {
        /// <summary>
        /// 构造函数，定义更新命令的验证规则
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public UpdateLanguageInfoCommandValidator(ILogger<UpdateLanguageInfoCommand> logger)
        {
            // code 字段必须非空
            RuleFor(command => command.code).NotEmpty();
        }
    }
}