using YS.CoffeeMachine.Application.Commands.InternalMsgCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.InternalMsg;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.InternalMsgCommandsHandlers
{
    /// <summary>
    /// 处理用户消息相关命令
    /// </summary>
    /// <param name="context"></param>
    public class UserMessagesCommandsHandlers(CoffeeMachinePlatformDbContext context) : ICommandHandler<UserMessagesCommands>
    {
        /// <summary>
        /// 处理用户消息相关命令
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(UserMessagesCommands request, CancellationToken cancellationToken)
        {
            // 验证输入参数
            if (request.dto == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0011)]);

            var userMessage = new UserMessages(request.dto.MessageId, request.dto.UserId, request.dto.IsRead, request.dto.ReadTime, request.dto.IsPopupShown);
            await context.UserMessages.AddAsync(userMessage);
        }
    }

    /// <summary>
    /// 标记用户消息为已读处理器
    /// </summary>
    /// <param name="context"></param>
    public class MarkAsReadCommandsHandlers(CoffeeMachinePlatformDbContext context) : ICommandHandler<MarkAsReadCommands>
    {
        /// <summary>
        /// 标记用户消息为已读处理器
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(MarkAsReadCommands request, CancellationToken cancellationToken)
        {
            // 验证输入参数
            if (request.Id <= 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0013)]);
            var userMessage = await context.UserMessages.FindAsync(request.Id);
            if (userMessage == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            userMessage.MarkAsRead();
            context.UserMessages.Update(userMessage);
        }
    }

    /// <summary>
    /// 标记用户消息为已弹窗处理器
    /// </summary>
    /// <param name="context"></param>
    public class MarkAsPopupShownCommandsHandlers(CoffeeMachinePlatformDbContext context) : ICommandHandler<MarkAsPopupShownCommands>
    {
        /// <summary>
        /// 标记用户消息为已弹窗处理器
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(MarkAsPopupShownCommands request, CancellationToken cancellationToken)
        {
            // 验证输入参数
            if (request.Id <= 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0013)]);
            var userMessage = await context.UserMessages.FindAsync(request.Id);
            if (userMessage == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            userMessage.MarkAsPopupShown();
            context.UserMessages.Update(userMessage);
        }
    }
}