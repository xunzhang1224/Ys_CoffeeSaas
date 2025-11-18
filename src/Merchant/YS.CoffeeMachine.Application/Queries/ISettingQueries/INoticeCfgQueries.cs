using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.Notity;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Domain.AggregatesModel.Basics;
using YS.CoffeeMachine.Domain.AggregatesModel.Settings;

namespace YS.CoffeeMachine.Application.Queries.ISettingQueries
{
    /// <summary>
    /// 通知查询接口
    /// </summary>
    public interface INoticeCfgQueries
    {
        /// <summary>
        /// 查询通知
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<List<NotityCfgOutput>> GetNoticeCfgsAsync(int type);

        /// <summary>
        /// 通知
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<PagedResultDto<NotityMsg>> GetNotityMsgAsync(NotityMsgInput input);
    }
}
