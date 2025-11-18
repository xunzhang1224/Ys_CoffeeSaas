using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.GroupsCommands;
using YS.CoffeeMachine.Domain.IRepositories.DevicesIRepositories;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YS.Provider.OSS.Model;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.DevicesCommandHandlers.GroupsCommandHandler
{
    /// <summary>
    /// 编辑分组
    /// </summary>
    /// <param name="repository"></param>
    public class UpdateGroupsCommandHandler(IGroupsRepository repository, CoffeeMachineDbContext context) : ICommandHandler<UpdateGroupsCommand, bool>
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

            string newParentPath = string.Empty;
            string oldPath = info.Path;

            // 判断层级是否改变
            if (info.PId == request.pid)
            {
                // 层级未改变,无需其他特殊处理
            }
            else
            {
                // 先获取所有子分组
                var sonGroups = await context.Groups.Where(w => w.Path.Contains(oldPath) && !w.IsDelete && w.Id != info.Id).ToListAsync();

                if (request.pid != null && request.pid > 0)
                {
                    ////获取所有分组
                    //var allGroups = await context.Groups.Where(w => !w.IsDelete).ToListAsync();
                    //if (request.pid != null && info.IsInvalidParent(request.pid.Value, allGroups))
                    //    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0070)]);

                    //// 获取父级信息
                    //var parentHierarchy = info.GetParentHierarchy(request.pid.Value, allGroups);

                    //// 获取子级信息
                    //var childHierarchy = info.GetChildHierarchy(request.id, allGroups);
                    //// 计算层级数：父级层级数 + 当前节点 + 子级层级数
                    //int levelCount = parentHierarchy.Count + 1 + childHierarchy.Count;
                    //if (levelCount > 8)
                    //    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0069)]);

                    if (sonGroups.Count > 0 && sonGroups.Any(a => a.Id == request.pid))
                    {
                        throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0070)]);
                    }
                    else
                    {
                        var parentGroup = await context.Groups.Where(w => w.Id == request.pid).FirstOrDefaultAsync();
                        var rootLevel = oldPath.Split('.').Length;
                        var level = sonGroups.Count == 0 ? 1 : sonGroups.Max(a => a.Path.Split('.').Length) - rootLevel + 1;
                        if (parentGroup.Path.Split('.').Count() + level > 8)
                        {
                            throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0069)]);
                        }
                        else
                        {
                            newParentPath = parentGroup.Path;
                        }
                    }

                }

                if (newParentPath == string.Empty)
                {
                    foreach (var item in sonGroups)
                    {
                        item.UpdatePath(oldPath, info.Id.ToString());
                    }
                    info.UpdatePath(oldPath, info.Id.ToString());
                }
                else
                {
                    foreach (var item in sonGroups)
                    {
                        item.UpdatePath(oldPath, newParentPath + "." + info.Id.ToString());
                    }
                    info.UpdatePath(oldPath, newParentPath + "." + info.Id.ToString());
                }

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