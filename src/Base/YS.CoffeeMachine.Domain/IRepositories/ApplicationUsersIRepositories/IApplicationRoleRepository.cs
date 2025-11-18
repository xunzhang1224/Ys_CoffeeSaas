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
    public interface IApplicationRoleRepository : IYsRepository<ApplicationRole, long>
    {
        /// <summary>
        /// 根据id获取菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApplicationRole> GetByIdAsync(long id);

        /// <summary>
        /// 添加并绑定
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
