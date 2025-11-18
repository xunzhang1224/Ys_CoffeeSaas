using Microsoft.EntityFrameworkCore;
using Polly;
using System.Linq;
using YS.CoffeeMachine.Application.Commands.ApplicationCommands.EnterpriseCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.IRepositories.ApplicationUsersIRepositories;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.ApplicationCommandHandlers.EnterpriseCommandHandlers
{
    /// <summary>
    /// 更新企业信息
    /// </summary>
    /// <param name="repository"></param>
    public class UpdateEnterpriseInfoCommandHandler(IEnterpriseInfoRepository repository, CoffeeMachineDbContext context) : ICommandHandler<UpdateEnterpriseInfoCommand, bool>
    {
        /// <summary>
        /// 更新企业信息
        /// </summary>
        public async Task<bool> Handle(UpdateEnterpriseInfoCommand request, CancellationToken cancellationToken)
        {

            #region 判断名称是否重复

            // 获取根企业节点id
            var rootId = await GetRootIdAsync(request.pid ?? 0);

            // 获取树结构下所有名称
            var allNodes = await context.EnterpriseInfo
             .Where(w => w.Id != request.id)
             .AsNoTracking()
             .ToListAsync();

            var lookup = allNodes.ToLookup(x => x.Pid);
            List<string> GetAllDescendantNames(long id)
            {
                var result = new List<string>();
                void Recurse(long currentId)
                {
                    foreach (var child in lookup[currentId])
                    {
                        result.Add(child.Name);
                        Recurse(child.Id);
                    }
                }
                Recurse(id);
                return result;
            }

            var names = GetAllDescendantNames(rootId ?? 0);

            bool isDuplicate = names.Contains(request.name);
            if (isDuplicate)
            {
                throw ExceptionHelper.AppFriendly(string.Format(L.Text[nameof(ErrorCodeEnum.D0088)], request.name));
            }
            #endregion

            var info = await repository.GetByIdAsync(request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            if (request.pid == info.Id)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0015)]);

            #region 计算层级逻辑处理
            if (request.pid != null && request.pid > 0)
            {
                var pInfo = repository.Get(request.pid.Value);
                if (pInfo == null)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0011)]);

                //获取所有企业
                var allList = await repository.GetAllAsync();

                if (info.IsInvalidParent(request.pid.Value, allList))
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0016)]);

                // 获取父级信息
                var parentHierarchy = info.GetParentHierarchy(request.pid.Value, allList);

                // 获取子级信息
                var childHierarchy = info.GetChildHierarchy(request.id, allList);

                // 合并父子数据，父级在前，子级在后
                //var allData = parentHierarchy.Concat(childHierarchy).ToList();

                // 计算层级数：父级层级数 + 当前节点 + 子级层级数
                int levelCount = parentHierarchy.Count + 1 + childHierarchy.Count;
                if (levelCount > 6)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0012)]);
            }
            #endregion

            // 修改企业信息
            info.Update(request.name, request.enterpriseTypeId, request.pid, request.remark, request.areaRelationId);
            // 需要改企业用户
            //if (request.userIds != null)
            //{
            //    //var userids = info.Users.Select(s => s.UserId).Union(request.userIds).ToList();
            //    //info.UpdateEnterpriseUsers(userids);
            //    info.UpdateEnterpriseUsers(request.userIds);
            //}

            // 需要绑定企业管理员权限的企业用户
            if (request.userIds != null)
            {
                // 获取企业管理与角色Id
                var adminRoleId = await context.ApplicationRole
                    .Where(x => x.Code == "administrator" && x.SysMenuType == SysMenuTypeEnum.Merchant)
                    .Select(s => s.Id)
                    .FirstOrDefaultAsync();

                //var aa = info.Users.Where(w => w.User.ApplicationUserRoles.Select(s => s.RoleId).Contains(adminRoleId)).Select(s => s.UserId).ToList();
                var needClearUserRoles = info.Users.Where(w => w.User.ApplicationUserRoles.Select(s => s.RoleId).Contains(adminRoleId)).Select(s => s.UserId).ToList();

                // 当前企业下拥有企业管理员角色的用户角色关系全清掉
                await context.ApplicationUserRole
                    .Where(x => needClearUserRoles.Contains(x.UserId) && x.Role.SysMenuType == SysMenuTypeEnum.Merchant && x.RoleId == adminRoleId)
                    .ExecuteDeleteAsync();

                // 更新当前企业的企业管理员权限
                var userRoles = request.userIds.Select(userId => new ApplicationUserRole(userId, adminRoleId));
                // 添加或更新企业管理员角色
                await context.ApplicationUserRole
                    .AddRangeAsync(userRoles);
            }

            //需要改企业角色
            if (request.roleIds != null)
            {
                var roleids = info.Roles.Select(s => s.RoleId).ToList().Union(request.roleIds).ToList();
                info.UpdateEnterpriseRoles(roleids);
            }
            //提交数据
            var res = await repository.UpdateAsync(info);
            await context.SaveChangesAsync();
            return res != null;
        }

        /// <summary>
        /// 获取根企业节点id
        /// </summary>
        /// <param name="currentId"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<long?> GetRootIdAsync(long currentId)
        {
            var visited = new HashSet<long>(); // 防止死循环

            while (true)
            {
                if (visited.Contains(currentId))
                    throw new InvalidOperationException("循环引用检测：企业树存在环。");

                visited.Add(currentId);

                var node = await context.EnterpriseInfo
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == currentId);

                if (node == null || node.Pid == null)
                    return node?.Id;

                currentId = node.Pid.Value;
            }
        }
    }
}
