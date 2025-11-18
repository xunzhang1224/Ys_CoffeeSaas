using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.API.Queries.Pagings;
using YS.CoffeeMachine.Application.Dtos.InternalMsgDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Queries.IInternalMsgQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.InternalMsg;
using YS.CoffeeMachine.Domain.Shared.Enum;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Queries.InternalMsgQueries
{
    /// <summary>
    /// 系统公告查询
    /// </summary>
    /// <param name="context"></param>
    /// <param name="_user"></param>
    public class SystemMessagesQueries(CoffeeMachineDbContext context, UserHttpContext _user) : ISystemMessagesQueries
    {
        /// <summary>
        /// 查询系统公告分页结果
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResultDto<SystemMessagesDto>> GetPagedResultAsync(SystemMessagesInput input)
        {
            var data = await context.SystemMessages.AsQueryable().AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Title), w => w.Title.Contains(input.Title!))
                .WhereIf(input.DateTimes != null && input.DateTimes.Count() == 2, w => w.CreateTime >= input.DateTimes![0] && w.CreateTime <= input.DateTimes[1])
                .Where(w => w.MessageType == InternalMsgEnum.Global)
                .Select(s => new SystemMessagesDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    MessageType = s.MessageType,
                    TargetUserId = s.TargetUserId,
                    TargetGroupText = s.TargetGroup,
                    IsPopup = s.IsPopup,
                    Priority = s.Priority,
                    ExpireTime = s.ExpireTime,
                    CreateTime = s.CreateTime
                })
                .ToPagedListAsync(input);

            // 获取当前页公告ID集合
            var msgIds = data.Items.Select(s => s.Id).ToList();

            // 获取用户读取公告的状态
            var UserReadGlobalMessages = await context.UserReadGlobalMessages.AsQueryable().AsNoTracking()
                .Where(w => w.UserId == _user.UserId && msgIds.Contains(w.MessageId))
                .Select(s => s.MessageId)
                .ToListAsync();

            // 标记用户是否已读
            data.Items.ForEach(item =>
            {
                item.IsRead = UserReadGlobalMessages.Contains(item.Id);
            });

            return data;
        }

        /// <summary>
        /// 根据ID获取系统公告详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<SystemMessagesDto> GetSystemMessagesByIdAsync(long id)
        {
            var data = await context.SystemMessages.AsNoTracking()
                .Where(w => w.Id == id)
                .Select(s => new SystemMessagesDto
                {
                    Title = s.Title,
                    Content = s.Content,
                    MessageType = s.MessageType,
                    TargetUserId = s.TargetUserId,
                    TargetGroupText = s.TargetGroup,
                    IsPopup = s.IsPopup,
                    Priority = s.Priority,
                    ExpireTime = s.ExpireTime,
                    CreateTime = s.CreateTime,
                    IsRead = true
                })
                .FirstOrDefaultAsync();

            if (data != null && _user.UserId > 0)
            {
                // 获取用户是否已读
                var isRead = await context.UserReadGlobalMessages.AsNoTracking()
                    .AnyAsync(w => w.UserId == _user.UserId && w.MessageId == id);

                // 如果用户未读，则设置为已读状态
                if (!isRead)
                {
                    var userreadGlobalMessages = new UserReadGlobalMessages(id, _user.UserId);
                    await context.UserReadGlobalMessages.AddAsync(userreadGlobalMessages);
                    await context.SaveChangesAsync();
                }
            }

            return data ?? throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0000)]);
        }
    }
}