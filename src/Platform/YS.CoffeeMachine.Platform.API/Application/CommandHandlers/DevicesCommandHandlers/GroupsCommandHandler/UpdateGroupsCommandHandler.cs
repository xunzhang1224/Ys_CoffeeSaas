using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.GroupsCommands;
using YS.CoffeeMachine.Domain.IPlatformRepositories.DevicesIRepositories;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.DevicesCommandHandlers.GroupsCommandHandler
{
    /// <summary>
    /// 编辑分组
    /// </summary>
    /// <param name="repository"></param>
    public class UpdateGroupsCommandHandler(IPGroupsRepository repository, CoffeeMachinePlatformDbContext context) : ICommandHandler<UpdateGroupsCommand, bool>
    {
        /// <summary>
        /// 编辑分组
        /// </summary>
        public async Task<bool> Handle(UpdateGroupsCommand request, CancellationToken cancellationToken)
        {
            var info = await repository.GetByIdAsync(request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            if (request.pid == info.Id)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0015)]);

            if (request.pid != null && request.pid > 0)
            {
                //获取所有分组
                var allGroups = await context.Groups.Where(w => !w.IsDelete).ToListAsync();
                if (request.pid != null && info.IsInvalidParent(request.pid.Value, allGroups))
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0070)]);

                // 获取父级信息
                var parentHierarchy = info.GetParentHierarchy(request.pid.Value, allGroups);

                // 获取子级信息
                var childHierarchy = info.GetChildHierarchy(request.id, allGroups);
                // 计算层级数：父级层级数 + 当前节点 + 子级层级数
                int levelCount = parentHierarchy.Count + 1 + childHierarchy.Count;
                if (levelCount > 8)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0069)]);
            }

            info.Update(request.name, request.pid, request.Remark);
            //绑定用户
            if (request.userIds != null)
                info.BindUsers(request.userIds.Distinct().ToList());
            //绑定设备
            if (request.deviceIds != null)
                info.BindDevices(request.deviceIds.Distinct().ToList());
            var res = await repository.UpdateAsync(info);
            return res != null;
        }

    }
}