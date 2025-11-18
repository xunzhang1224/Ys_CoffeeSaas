using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.InternalMsgCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.InternalMsg;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.InternalMsgCommandsHandlers
{
    /// <summary>
    /// 执行系统消息相关命令处理
    /// </summary>
    /// <param name="context"></param>
    public class SystemMessagesCommandsHandlers(CoffeeMachinePlatformDbContext context) : ICommandHandler<SystemMessagesCommands>
    {
        /// <summary>
        /// 创建消息处理器
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Handle(SystemMessagesCommands request, CancellationToken cancellationToken)
        {
            // 验证输入参数
            if (request.content == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0011)]);

            var targetGroup = string.Empty;
            if (request.messageType == InternalMsgEnum.Group)
            {
                if (request.targetGroup == null || request.targetGroup.Count == 0)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0011)]);
                targetGroup = string.Join(",", request.targetGroup);
            }
            var info = new SystemMessages(request.title, request.content, request.messageType, request.targetUserId, targetGroup, request.isPopup, request.priority, request.expireTime);

            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                await context.SystemMessages.AddAsync(info);
                await context.SaveChangesAsync(cancellationToken);

                // 如果是分组公告，给分组下所有用户添加一条记录
                if (request.messageType == InternalMsgEnum.Group)
                {
                    //  获取企业信息
                    var companyIds = await context.EnterpriseInfo
                        .Where(w => request.targetGroup!.Contains(w.Id))
                        .Select(s => s.Id)
                        .ToListAsync();

                    if (companyIds == null || companyIds.Count == 0)
                        throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0068)]);

                    //  企业下获取所有用户ID
                    var allUserIds = await context.ApplicationUser.AsQueryable().AsNoTracking()
                        .Where(w => companyIds.Contains(w.EnterpriseId))
                        .Select(s => s.Id)
                        .ToListAsync(cancellationToken);

                    // 组装用户消息记录
                    var userMessages = new List<UserMessages>();
                    foreach (var userId in allUserIds)
                    {
                        userMessages.Add(new UserMessages(info.Id, userId, false, null, request.isPopup));
                    }

                    // 批量插入用户消息记录
                    await context.BulkInsertAsync(userMessages);
                }
                // 如果是用户消息，直接添加一条记录
                else if (request.messageType == InternalMsgEnum.Private)
                {
                    if (request.targetUserId == null || request.targetUserId == 0)
                        throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0011)]);

                    var userMessage = new UserMessages(info.Id, request.targetUserId ?? 0, false, null, request.isPopup);
                    await context.UserMessages.AddAsync(userMessage);
                }
                //全局公告不需要单独添加用户消息记录

                // 提交事务
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                // 回滚事务
                await transaction.RollbackAsync();
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0000)]);
            }
        }
    }

    /// <summary>
    /// 编辑系统消息处理器
    /// </summary>
    /// <param name="context"></param>
    public class UpdateSystemMessagesCommandsHandlers(CoffeeMachinePlatformDbContext context) : ICommandHandler<UpdateSystemMessagesCommands>
    {
        /// <summary>
        /// 编辑系统消息处理器
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(UpdateSystemMessagesCommands request, CancellationToken cancellationToken)
        {
            // 验证输入参数
            if (request.id <= 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0013)]);
            var info = await context.SystemMessages.FindAsync(request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            info.Update(request.title, request.content);
            context.SystemMessages.Update(info);
        }
    }

    /// <summary>
    /// 取消系统消息处理器
    /// </summary>
    /// <param name="context"></param>
    public class CancelSystemMessagesCommandsHandlers(CoffeeMachinePlatformDbContext context) : ICommandHandler<CancelSystemMessagesCommands>
    {
        /// <summary>
        /// 取消系统消息处理器
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(CancelSystemMessagesCommands request, CancellationToken cancellationToken)
        {
            // 验证输入参数
            if (request.id <= 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0013)]);
            var info = await context.SystemMessages.FindAsync(request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            info.Cancel();
            context.SystemMessages.Update(info);
        }
    }
}