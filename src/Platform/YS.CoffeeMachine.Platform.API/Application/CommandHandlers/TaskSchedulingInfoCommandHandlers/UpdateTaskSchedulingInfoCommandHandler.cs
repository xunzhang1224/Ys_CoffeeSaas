using YS.CoffeeMachine.Application.Commands.TaskSchedulingInfoCommands;
using YS.CoffeeMachine.Domain.IPlatformRepositories;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.TaskSchedulingInfoCommandHandlers
{
    /// <summary>
    /// 编辑任务调度信息
    /// </summary>
    /// <param name="repository"></param>
    public class UpdateTaskSchedulingInfoCommandHandler(IPTaskSchedulingInfoRepository repository) : ICommandHandler<UpdateTaskSchedulingInfoCommand, bool>
    {
        /// <inheritdoc/>
        public async Task<bool> Handle(UpdateTaskSchedulingInfoCommand request, CancellationToken cancellationToken)
        {
            var info = await repository.GetAsync(request.id);
            info.Update(request.name, request.description, request.cron, request.isEnabled);
            var res = await repository.UpdateAsync(info);
            return res != null;
        }
    }
}
