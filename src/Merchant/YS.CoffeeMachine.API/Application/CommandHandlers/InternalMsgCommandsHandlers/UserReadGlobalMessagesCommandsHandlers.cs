using YS.CoffeeMachine.Application.Commands.InternalMsgCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.InternalMsg;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.InternalMsgCommandsHandlers
{
    /// <summary>
    /// 用户读取全局消息命令处理器
    /// </summary>
    /// <param name="context"></param>
    public class UserReadGlobalMessagesCommandsHandlers(CoffeeMachinePlatformDbContext context) : ICommandHandler<UserReadGlobalMessagesCommands>
    {
        /// <summary>
        /// 用户读取全局消息命令处理器
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(UserReadGlobalMessagesCommands request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0011)]);
            var info = new UserReadGlobalMessages(request.messageId, request.userId);
            await context.UserReadGlobalMessages.AddAsync(info);
        }
    }
}
