using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.DeviceDots;

namespace YS.CoffeeMachine.Application.Queries.IDevicesQueries
{
    /// <summary>
    /// 设备预警配置查询
    /// </summary>
    public interface IEarlyWarningConfigQueries
    {
        /// <summary>
        /// 获取设备预警配置列表
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        Task<EarlyWarningConfigDto> GetEarlyWarningConfigByIdAsync(long deviceId);
    }
}
