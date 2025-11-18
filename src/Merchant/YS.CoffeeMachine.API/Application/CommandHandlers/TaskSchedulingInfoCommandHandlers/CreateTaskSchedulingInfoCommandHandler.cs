using YS.CoffeeMachine.Application.Commands.TaskSchedulingInfoCommands;
using YS.CoffeeMachine.Domain.AggregatesModel;
using YS.CoffeeMachine.Domain.IRepositories;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.TaskSchedulingInfoCommandHandlers
{
    /// <summary>
    /// 创建任务调度信息
    /// </summary>
    /// <param name="repository"></param>
    public class CreateTaskSchedulingInfoCommandHandler(ITaskSchedulingInfoRepository repository) : ICommandHandler<CreateTaskSchedulingInfoCommand, bool>
    {
        /// <inheritdoc/>
        public async Task<bool> Handle(CreateTaskSchedulingInfoCommand request, CancellationToken cancellationToken)
        {
            var taskSchedulingInfo = new TaskSchedulingInfo(request.name, request.description, request.cron, request.isEnabled);
            var res = await repository.AddAsync(taskSchedulingInfo);
            return res != null;
        }
    }
}
