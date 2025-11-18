using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.ApplicationCommands.EnterpriseCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.IPlatformRepositories.ApplicationUsersIRepositories;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.ApplicationCommandHandlers.EnterpriseCommandHandlers
{
    /// <summary>
    /// 商户端添加企业
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="context"></param>
    public class CreateEnterpriseInfoCommandHandler(IPEnterpriseInfoRepository repository, CoffeeMachinePlatformDbContext context) : ICommandHandler<CreateEnterpriseInfoCommand, bool>
    {
        /// <summary>
        /// 商户端添加企业
        /// </summary>
        public async Task<bool> Handle(CreateEnterpriseInfoCommand request, CancellationToken cancellationToken)
        {
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
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

            // 企业绑定菜单Ids
            var allMenus = string.IsNullOrWhiteSpace(curEnterpriseInfo.MenuIds) ? [] : curEnterpriseInfo.MenuIds.Split(",").Select(s => Convert.ToInt64(s)).ToList();
            info.UpdateMenuIds(allMenus);

            // 企业绑定H5菜单Ids
            var allH5Menus = string.IsNullOrWhiteSpace(curEnterpriseInfo.H5MenuIds) ? [] : curEnterpriseInfo.H5MenuIds.Split(",").Select(s => Convert.ToInt64(s)).ToList();
            info.UpdateH5MenuIds(allH5Menus);

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
    }
}
