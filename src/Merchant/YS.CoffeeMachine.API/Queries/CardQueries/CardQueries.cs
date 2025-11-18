using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.API.Queries.Pagings;
using YS.CoffeeMachine.Application.Dtos.Card;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Queries.CardQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.Card;
using YS.CoffeeMachine.Infrastructure;

namespace YS.CoffeeMachine.API.Queries.CardQueries
{
    /// <summary>
    /// 卡管理
    /// </summary>
    /// <param name=""></param>
    public class CardQueries(CoffeeMachineDbContext _db) : ICardQueries
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<CardInfo>> GetCardInfosAsync(CardDto query)
        {
            return await _db.CardInfo.AsNoTracking().WhereIf(!string.IsNullOrWhiteSpace(query.CardNumber), w => w.CardNumber.Contains(query.CardNumber)).OrderByDescending(x => x.CreateTime).ToPagedListAsync(query);
        }

        /// <summary>
        /// 获取设备绑定卡
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<List<CardInfo>> GetCardInfosByDeviceIdAsync(long query)
        {
            return await _db.CardInfo
            .Include(c => c.Assignments)
            .Where(c => c.IsEnabled &&
                       c.Assignments.Any(a => a.DeviceId == query))
            .ToListAsync();
        }

        /// <summary>
        ///  查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<long>> GetCardInfosByIdAsync(long id)
        {
            return await (from card in _db.CardInfo
                          join assignment in _db.CardDeviceAssignment
                          on card.Id equals assignment.CardId into assignments
                          from assignment in assignments.DefaultIfEmpty()
                          where card.Id == id && assignment != null
                          select assignment.DeviceId).Distinct()
                  .ToListAsync();
            //return await _db.CardInfo.Include(x => x.Assignments).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
