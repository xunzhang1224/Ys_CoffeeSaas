using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YSCore.Base.DatabaseAccessor.Repositories;

namespace YS.CoffeeMachine.Domain.IPlatformRepositories.ApplicationUsersIRepositories
{
    /// <summary>
    /// 应用菜单仓储
    /// </summary>
    public interface IPApplicationRoleRepository : IYsRepository<ApplicationRole, long>
    {
        /// <summary>
        /// GetByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApplicationRole> GetByIdAsync(long id);

        /// <summary>
        /// 添加并绑定角色菜单
        /// </summary>
        /// <param name="name"></param>
        /// <param name="roleStatus"></param>
        /// <param name="sysMenuType"></param>
        /// <param name="hasSuperAdmin"></param>
        /// <param name="sort"></param>
        /// <param name="remark"></param>
        /// <param name="menuIds"></param>
        /// <param name="enterpriseId"></param>
        /// <returns></returns>

        Task<bool> AddAndBindAsync(string name, RoleStatusEnum roleStatus, SysMenuTypeEnum sysMenuType, bool? hasSuperAdmin, int sort, string remark, List<long>? menuIds, long enterpriseId);
    }
}
