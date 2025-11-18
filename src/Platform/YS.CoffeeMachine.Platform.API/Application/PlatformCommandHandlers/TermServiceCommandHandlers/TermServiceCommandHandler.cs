using YS.CoffeeMachine.Application.Commands.TermServiceCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Strategy;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.TermServiceCommandHandlers
{
    /// <summary>
    /// 创建服务条款命令处理类
    /// </summary>
    public class CreateTermServiceCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<CreateTermServiceCommand, bool>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(CreateTermServiceCommand request, CancellationToken cancellationToken)
        {
            var info = new TermServiceEntity(request.title, request.content, request.description, request.enabled);
            await context.AddAsync(info);
            return true;
        }
    }

    /// <summary>
    /// 编辑服务条款命令处理类
    /// </summary>
    /// <param name="context"></param>
    public class UpdateTermServiceCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<UpdateTermServiceCommand, bool>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(UpdateTermServiceCommand request, CancellationToken cancellationToken)
        {
            var info = await context.TermServiceEntity.FindAsync(request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

            info.Update(request.title, request.content, request.description, request.enabled);
            context.Update(info);
            return true;
        }
    }

    /// <summary>
    /// 删除服务条款命令处理类
    /// </summary>
    /// <param name="context"></param>
    public class DeleteTermServiceCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<DeleteTermServiceCommand, bool>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(DeleteTermServiceCommand request, CancellationToken cancellationToken)
        {
            await context.TermServiceEntity.FakeDeleteAsync(request.id);
            return true;
        }
    }

    /// <summary>
    /// 变更服务条款状态命令处理类
    /// </summary>
    /// <param name="context"></param>
    public class ChangeTermServiceStatusCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<ChangeTermServiceStatusCommand, bool>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(ChangeTermServiceStatusCommand request, CancellationToken cancellationToken)
        {
            var info = await context.TermServiceEntity.FindAsync(request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            info.SetEnabled(request.enabled);
            context.Update(info);
            return true;
        }
    }
}