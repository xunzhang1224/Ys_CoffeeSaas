using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YSCore.Base.DatabaseAccessor.Repositories;

namespace YS.CoffeeMachine.Domain.IRepositories.ApplicationUsersIRepositories
{
    /// <summary>
    /// 用户信息仓储
    /// </summary>
    public interface IApplicationUserRepository : IYsRepository<ApplicationUser, long>
    {
        /// <summary>
        /// 通过Id获取用户信息
        /// </summary>
        Task<ApplicationUser> GetByIdAsync(long id);

        /// <summary>
        /// 添加并绑定用户信息
        /// </summary>
        Task<bool> AddAndBindAsync(long enterpriseId, string account, string password, string nickName, string areaCode, string phone, string email, UserStatusEnum status, AccountTypeEnum accountType, SysMenuTypeEnum sysMenuType, string remark, List<long>? roleIds);
    }
}
