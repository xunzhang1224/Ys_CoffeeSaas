using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.API.Queries.Pagings;
using YS.CoffeeMachine.Application.Dtos.Notity;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Queries.IDevicesQueries;
using YS.CoffeeMachine.Application.Queries.ISettingQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
using YS.CoffeeMachine.Infrastructure;

namespace YS.CoffeeMachine.API.Queries.SettingQueries
{

    /// <summary>
    /// a
    /// </summary>
    public class NoticeCfgQueries(CoffeeMachineDbContext context,UserHttpContext _userHttpContext,IDeviceInfoQueries _deviceInfoQueries) : INoticeCfgQueries
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<List<NotityCfgOutput>> GetNoticeCfgsAsync(int type)
        {
            var query = from user in context.ApplicationUser
                        join noticeCfg in context.NoticeCfg.Where(n => n.Type == type)  // 只关联Type=0的记录
                            on user.Id equals noticeCfg.UserId
                            into noticeCfgs
                        from noticeCfg in noticeCfgs.DefaultIfEmpty()
                        where user.EnterpriseId == _userHttpContext.TenantId
                        select new NotityCfgOutput
                        {
                            UserId = user.Id,
                            UserName = user.NickName,
                            Type = type,
                            Status = noticeCfg != null ? noticeCfg.Status : false,
                            Method = noticeCfg != null ? noticeCfg.Method : null
                        };

            return await query
                .WhereIf(_userHttpContext.AllDeviceRole == false, x => x.UserId == _userHttpContext.UserId)
                .ToListAsync();
        }

        /// <summary>
        /// 查询消息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTimem"></param>
        /// <param name="contactAddress"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResultDto<NotityMsg>> GetNotityMsgAsync(NotityMsgInput input)
        {
            return await context.NotityMsg.AsQueryable()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), x => x.MsgName.Contains(input.Name))
                .WhereIf(input.Type != null, x => x.Type == input.Type)
                .WhereIf(input.Times != null && input.Times.Count == 2, x => x.CreateTime >= input.Times[0] && x.CreateTime <= input.Times[1])
                .WhereIf(!string.IsNullOrWhiteSpace(input.contactAddress), x => x.ContactAddress.Contains(input.contactAddress))
                .WhereIf(_userHttpContext.AllDeviceRole == false, x => x.ContactAddress == _userHttpContext.Account)
                .OrderByDescending(x => x.CreateTime)
                .ToPagedListAsync(input);
        }
    }
}
