using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.Card;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.AggregatesModel.Card;

namespace YS.CoffeeMachine.Application.Queries.CardQueries
{
    /// <summary>
    /// 卡查询接口
    /// </summary>
    public interface ICardQueries
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        Task<PagedResultDto<CardInfo>> GetCardInfosAsync(CardDto query);

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        Task<List<CardInfo>> GetCardInfosByDeviceIdAsync(long query);

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<long>> GetCardInfosByIdAsync(long id);
    }
}
