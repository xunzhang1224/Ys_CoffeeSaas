using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.ApplicationCommands.EnterpriseCommands;
using YS.CoffeeMachine.Application.IServices;
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
    /// 商户端添加企业
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="context"></param>
    public class CreateEnterpriseInfoCommandHandler(IEnterpriseInfoRepository repository, CoffeeMachineDbContext context, IAliyunSmsService aliyunSmsService) : ICommandHandler<CreateEnterpriseInfoCommand, bool>
    {
        /// <summary>
        /// 商户端添加企业
        /// </summary>
        public async Task<bool> Handle(CreateEnterpriseInfoCommand request, CancellationToken cancellationToken)
        {

            #region 判断名称是否重复

            // 获取根企业节点id
            var rootId = await GetRootIdAsync(request.pid);

            // 获取树结构下所有名称
            var allNodes = await context.EnterpriseInfo
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

            var info = new EnterpriseInfo(request.name, request.enterpriseTypeId, request.pid, request.remark, request.userIds, request.roleIds, null, null, null, request.areaRelationId);
            var parentEnterprise = await context.EnterpriseInfo.AsNoTracking().FirstOrDefaultAsync(w => w.Id == request.pid);
            if (parentEnterprise == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0011)]);

            // 获取父级信息
            var parentHierarchyCount = await GetParentHierarchy(request.pid);// info.GetParentHierarchy(request.pid, allList);

            // 计算层级数：父级层级数 + 当前节点
            int levelCount = parentHierarchyCount.Item1 + 2;
            if (levelCount > 6)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0012)]);

            // 获取上级企业所有菜单Ids
            var curEnterpriseInfo = await context.EnterpriseInfo.AsNoTracking().FirstOrDefaultAsync(w => w.Id == request.pid);
            if (curEnterpriseInfo == null)

                // 企业绑定菜单Ids
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0000)]);
            var allMenus = string.IsNullOrWhiteSpace(curEnterpriseInfo.MenuIds) ? [] : curEnterpriseInfo.MenuIds.Split(",").Select(s => Convert.ToInt64(s)).ToList();
            var halfMenus = string.IsNullOrWhiteSpace(curEnterpriseInfo.HalfMenuIds) ? [] : curEnterpriseInfo.HalfMenuIds.Split(",").Select(s => Convert.ToInt64(s)).ToList();
            info.UpdateMenuIds(allMenus, halfMenus);

            // 企业绑定H5菜单Ids
            var allH5Menus = string.IsNullOrWhiteSpace(curEnterpriseInfo.H5MenuIds) ? [] : curEnterpriseInfo.H5MenuIds.Split(",").Select(s => Convert.ToInt64(s)).ToList();
            var halfH5Menus = string.IsNullOrWhiteSpace(curEnterpriseInfo.H5HalfMenuIds) ? [] : curEnterpriseInfo.H5HalfMenuIds.Split(",").Select(s => Convert.ToInt64(s)).ToList();
            info.UpdateH5MenuIds(allH5Menus, halfH5Menus);

            var res = repository.Add(info);
            return res != null;
        }

        /// <summary>
        /// 获取上级总数
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public async Task<(int, long)> GetParentHierarchy(long pid)
        {
            var count = 0;
            var maxParentId = pid;
            var current = await context.EnterpriseInfo.AsNoTracking().FirstOrDefaultAsync(w => w.Id == pid);
            while (current != null && current.Pid.HasValue)
            {
                count++;
                maxParentId = current.Pid.Value;
                current = await context.EnterpriseInfo.AsNoTracking().FirstOrDefaultAsync(w => w.Id == current.Pid);
            }
            return (count, maxParentId);
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
