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
    /// 更新企业信息
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="context"></param>
    public class UpdateEnterpriseInfoCommandHandler(IPEnterpriseInfoRepository repository, CoffeeMachinePlatformDbContext context) : ICommandHandler<UpdateEnterpriseInfoCommand, bool>
    {
        /// <summary>
        /// 更新企业信息
        /// </summary>
        public async Task<bool> Handle(UpdateEnterpriseInfoCommand request, CancellationToken cancellationToken)
        {
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

            //修改企业信息
            info.Update(request.name, request.enterpriseTypeId, request.pid, request.remark, request.areaRelationId);
            //需要改企业用户
            if (request.userIds != null)
            {
                var userids = info.Users.Select(s => s.UserId).Union(request.userIds).ToList();
                info.UpdateEnterpriseUsers(userids);
            }
            //需要改企业角色
            if (request.roleIds != null)
            {
                var roleids = info.Roles.Select(s => s.RoleId).ToList().Union(request.roleIds).ToList();
                info.UpdateEnterpriseRoles(roleids);
            }
            //提交数据
            var res = await repository.UpdateAsync(info);
            return res != null;
        }
    }
}