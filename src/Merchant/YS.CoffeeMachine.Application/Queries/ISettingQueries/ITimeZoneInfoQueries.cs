using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.SettringDtos;

namespace YS.CoffeeMachine.Application.Queries.ISettingQueries
{
    /// <summary>
    /// 时区信息查询
    /// </summary>
    public interface ITimeZoneInfoQueries
    {
        /// <summary>
        /// 获取所有时区信息
        /// </summary>
        /// <returns></returns>
        Task<List<TimeZoneInfoDto>> GetAllTimeZoneInfoAsync();
    }
}
