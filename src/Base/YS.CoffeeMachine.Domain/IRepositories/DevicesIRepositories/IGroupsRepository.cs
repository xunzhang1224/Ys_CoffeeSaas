using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YSCore.Base.DatabaseAccessor.Repositories;

namespace YS.CoffeeMachine.Domain.IRepositories.DevicesIRepositories
{
    /// <summary>
    /// 设备分组
    /// </summary>
    public interface IGroupsRepository : IYsRepository<Groups, long>
    {
        /// <summary>
        /// 根据id获取分组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Groups> GetByIdAsync(long id);
    }
}
