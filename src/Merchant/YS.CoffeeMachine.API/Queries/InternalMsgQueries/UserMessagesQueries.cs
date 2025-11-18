using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.API.Queries.Pagings;
using YS.CoffeeMachine.Application.Dtos.InternalMsgDtos;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Queries.IInternalMsgQueries;
using YS.CoffeeMachine.Infrastructure;

namespace YS.CoffeeMachine.API.Queries.InternalMsgQueries
{
    /// <summary>
    /// 用户消息查询
    /// </summary>
    /// <param name="context"></param>
    /// <param name="_user"></param>
    public class UserMessagesQueries(CoffeeMachineDbContext context, UserHttpContext _user) : IUserMessagesQueries
    {
        /// <summary>
        /// 获取用户消息分页结果
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResultDto<UserMessagesDto>> GetPagedResultAsync(UserMessagesInput request)
        {
            return await context.UserMessages.AsQueryable().AsNoTracking()
                .WhereIf(request.IsRead != null, w => w.IsRead == request.IsRead)
                .Where(w => w.UserId == _user.UserId)
                .Select(s => new UserMessagesDto
                {
                    Id = s.Id,
                    MessageId = s.MessageId,
                    UserId = s.UserId,
                    IsRead = s.IsRead,
                    ReadTime = s.ReadTime,
                    IsPopupShown = s.IsPopupShown,
                })
                .ToPagedListAsync(request.PageNumber, request.PageSize);
        }

        /// <summary>
        /// 获取用户消息详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserMessagesDto> GetByIdAsync(int id)
        {
            return await context.UserMessages.AsQueryable().AsNoTracking()
                .Include(i => i.Message)
                .Where(w => w.Id == id && w.UserId == _user.UserId)
                .Select(s => new UserMessagesDto
                {
                    Id = s.Id,
                    MessageId = s.MessageId,
                    Content = s.Message.Content
                })
                .FirstOrDefaultAsync() ?? new UserMessagesDto();
        }
    }
}