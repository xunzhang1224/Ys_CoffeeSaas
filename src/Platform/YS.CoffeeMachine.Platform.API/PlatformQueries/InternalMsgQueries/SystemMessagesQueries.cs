using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Dtos.InternalMsgDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.PlatformQueries.IInternalMsgQueries;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Platform.API.Queries;
using YS.CoffeeMachine.Platform.API.Queries.Pagings;

namespace YS.CoffeeMachine.Platform.API.PlatformQueries.InternalMsgQueries
{
    /// <summary>
    /// 站内信查询服务
    /// </summary>
    public class SystemMessagesQueries(CoffeeMachinePlatformDbContext context) : ISystemMessagesQueries
    {
        /// <summary>
        /// 获取系统消息分页结果
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResultDto<SystemMessagesDto>> GetPagedResultAsync(SystemMessagesInput input)
        {
            return await context.SystemMessages.AsQueryable().AsNoTracking()
                .WhereIf(input.MessageType != null, w => w.MessageType == input.MessageType)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Title), w => w.Title.Contains(input.Title!))
                .WhereIf(input.TargetUserId != null && input.TargetUserId > 0, w => w.TargetUserId == input.TargetUserId)
                .WhereIf(input.DateTimes != null && input.DateTimes.Count() == 2, w => w.CreateTime >= input.DateTimes![0] && w.CreateTime <= input.DateTimes[1])
                .Select(s => new SystemMessagesDto
                {
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
        }

        /// <summary>
        /// 根据ID获取系统消息详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<SystemMessagesDto> GetSystemMessagesByIdAsync(long id)
        {
            return await context.SystemMessages.AsNoTracking()
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
                    CreateTime = s.CreateTime
                })
                .FirstOrDefaultAsync() ?? new SystemMessagesDto();
        }
    }
}