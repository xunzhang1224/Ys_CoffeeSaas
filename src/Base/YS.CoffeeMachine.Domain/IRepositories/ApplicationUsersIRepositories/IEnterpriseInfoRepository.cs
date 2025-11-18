using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YSCore.Base.DatabaseAccessor.Repositories;

namespace YS.CoffeeMachine.Domain.IRepositories.ApplicationUsersIRepositories
{
    /// <summary>
    /// 菜单仓储接口
    /// </summary>
    /// </summary>
    public interface IEnterpriseInfoRepository : IYsRepository<EnterpriseInfo, long>
    {
        /// <summary>
        /// 根据id获取菜单信息
        /// </summary>
        Task<EnterpriseInfo> GetByIdAsync(long id);

        /// <summary>
        /// 获取所有菜单信息
        /// </summary>

        Task<List<EnterpriseInfo>> GetAllAsync();
    }
}
