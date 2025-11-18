using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YSCore.Base.DatabaseAccessor.Repositories;

namespace YS.CoffeeMachine.Domain.IPlatformRepositories.DevicesIRepositories
{
    /// <summary>
    /// 设备型号仓储
    /// </summary>
    /// </summary>
    public interface IPGroupsRepository : IYsRepository<Groups, long>
    {
        /// <summary>
        /// GetByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Groups> GetByIdAsync(long id);
    }
}
