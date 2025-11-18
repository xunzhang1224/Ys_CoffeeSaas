using YS.CoffeeMachine.Application.Commands.TaskSchedulingInfoCommands;
using YS.CoffeeMachine.Domain.IPlatformRepositories;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.TaskSchedulingInfoCommandHandlers
{
    /// <summary>
    /// 删除任务调度信息
    /// </summary>
    public class DeleteTaskSchedulingInfoCommandHandler(IPTaskSchedulingInfoRepository repository) : ICommandHandler<DeleteTaskSchedulingInfoCommand, bool>
    {
        /// <inheritdoc/>
        public async Task<bool> Handle(DeleteTaskSchedulingInfoCommand request, CancellationToken cancellationToken)
        {
            if (request.id <= 0)
                throw ExceptionHelper.AppFriendly("Id不能为空{0}", request.id);
            var res = await repository.FakeDeleteByIdAsync(request.id);
            return res > 0;
        }
    }
}
