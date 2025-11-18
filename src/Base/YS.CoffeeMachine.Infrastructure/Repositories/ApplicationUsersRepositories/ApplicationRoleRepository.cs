using MediatR;
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
    /// 角色信息
    /// </summary>
    /// <param name="context"></param>
    public class ApplicationRoleRepository(CoffeeMachineDbContext context) : YsRepositoryBase<ApplicationRole, long, CoffeeMachineDbContext>(context), IApplicationRoleRepository
    {
        /// <summary>
        /// 根据Id查询角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApplicationRole> GetByIdAsync(long id)
        {
            return await context.ApplicationRole.Include(i => i.ApplicationRoleMenus).ThenInclude(i => i.Menu).FirstAsync(w => w.Id == id);
        }

        /// <summary>
        /// 添加角色信息
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
        public async Task<bool> AddAndBindAsync(string name, RoleStatusEnum roleStatus, SysMenuTypeEnum sysMenuType, bool? hasSuperAdmin, int sort, string remark, List<long>? menuIds, long enterpriseId)
        {
            //查询企业信息
            var enterpriseInfo = await context.EnterpriseInfo.FirstOrDefaultAsync(w => w.Id == enterpriseId);
            if (enterpriseInfo == null)
                throw ExceptionHelper.AppFriendly($"enterpriseId：{enterpriseId}未找到到企业数据");
            //添加角色信息
            var applicationRole = new ApplicationRole(name, roleStatus, sysMenuType, hasSuperAdmin, sort, remark, menuIds);
            var res = await context.AddAsync(applicationRole);
            if (res.Entity != null)
            {
                //绑定企业与角色关系
                if (enterpriseInfo != null)
                {
                    var curRoleIds = enterpriseInfo.Roles.Select(w => w.Id).ToList();
                    curRoleIds.Add(res.Entity.Id);
                    enterpriseInfo.UpdateEnterpriseRoles(curRoleIds);
                    context.Update(enterpriseInfo);
                    var updateRes = await context.SaveChangesAsync();
                    return updateRes > 0;
                }
            }
            return res != null;
        }
    }
}
