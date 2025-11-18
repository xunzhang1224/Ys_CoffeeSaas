using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.IRepositories.ApplicationUsersIRepositories;
using YSCore.Provider.EntityFrameworkCore;
using YSCore.Base.Exceptions;

namespace YS.CoffeeMachine.Infrastructure.Repositories.ApplicationUsersRepositories
{
    /// <summary>
    /// 用户信息
    /// </summary>
    /// <param name="context"></param>
    public class ApplicationUserRepository(CoffeeMachineDbContext context) : YsRepositoryBase<ApplicationUser, long, CoffeeMachineDbContext>(context), IApplicationUserRepository
    {
        /// <summary>
        /// 根据Id查询用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApplicationUser> GetByIdAsync(long id)
        {
            return await context.ApplicationUser.Include(i => i.ApplicationUserRoles).ThenInclude(i => i.Role).FirstAsync(w => w.Id == id);
        }

        /// <summary>
        /// 添加用户信息并绑定角色信息
        /// </summary>
        /// <param name="enterpriseId"></param>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="nickName"></param>
        /// <param name="areaCode"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="status"></param>
        /// <param name="accountType"></param>
        /// <param name="sysMenuType"></param>
        /// <param name="remark"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public async Task<bool> AddAndBindAsync(long enterpriseId, string account, string password, string nickName, string areaCode, string phone, string email, UserStatusEnum status, AccountTypeEnum accountType, SysMenuTypeEnum sysMenuType, string remark, List<long> roleIds)
        {
            //查询企业信息
            var enterpriseInfo = await context.EnterpriseInfo.FirstOrDefaultAsync(w => w.Id == enterpriseId);
            if (enterpriseInfo == null)
                throw ExceptionHelper.AppFriendly($"enterpriseId：{enterpriseId}未找到到企业数据");
            //添加用户信息
            var applicationUser = new ApplicationUser(enterpriseId, account, password, nickName, areaCode, phone, email, status, accountType, sysMenuType, remark, roleIds);
            var res = await context.ApplicationUser.AddAsync(applicationUser);
            if (res.Entity != null)
            {
                //绑定企业与用户关系
                var curUserIds = enterpriseInfo.Users.Select(w => w.Id).ToList();
                curUserIds.Add(res.Entity.Id);
                enterpriseInfo.UpdateEnterpriseUsers(curUserIds);
                context.Update(enterpriseInfo);
                var updateRes = await context.SaveChangesAsync();
                return updateRes > 0;
            }
            return false;
        }
    }
}
