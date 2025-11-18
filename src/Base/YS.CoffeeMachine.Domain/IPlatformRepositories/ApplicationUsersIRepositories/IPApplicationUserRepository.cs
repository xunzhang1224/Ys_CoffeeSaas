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
    /// 菜单仓储接口
    /// </summary>
    /// </summary>
    public interface IPApplicationUserRepository : IYsRepository<ApplicationUser, long>
    {
        /// <summary>
        /// 根据id获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApplicationUser> GetByIdAsync(long id);

        /// <summary>
        /// 添加并绑定
        /// </summary>
        Task<bool> AddAndBindAsync(long enterpriseId, string account, string password, string nickName, string areaCode, string phone, string email, UserStatusEnum status, AccountTypeEnum accountType, SysMenuTypeEnum sysMenuType, string remark, List<long>? roleIds);
    }
}
