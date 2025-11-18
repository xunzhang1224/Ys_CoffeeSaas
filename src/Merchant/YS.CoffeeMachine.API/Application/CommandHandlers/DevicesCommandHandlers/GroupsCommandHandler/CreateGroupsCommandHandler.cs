using Microsoft.EntityFrameworkCore;
using Yitter.IdGenerator;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.GroupsCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.IRepositories.DevicesIRepositories;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.DevicesCommandHandlers.GroupsCommandHandler
{
    /// <summary>
    /// 新增分组
    /// </summary>
    /// <param name="repository"></param>
    public class CreateGroupsCommandHandler(IGroupsRepository repository, CoffeeMachineDbContext context) : ICommandHandler<CreateGroupsCommand, bool>
    {
        /// <summary>
        /// 新增分组
        /// </summary>
        public async Task<bool> Handle(CreateGroupsCommand request, CancellationToken cancellationToken)
        {
            if (request.enterpriseInfoId <= 0)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0011)]);
            var enterpriseInfo = await context.EnterpriseInfo.FirstAsync(w => w.Id == request.enterpriseInfoId);
            if (enterpriseInfo == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0068)]);

            if (request.pid != null)
            {
                var parentGroup = await context.Groups.FirstOrDefaultAsync(w => w.Id == request.pid);
            }

            //var info = new Groups(request.enterpriseInfoId, request.name, request.pid, request.Remark, request.userIds, request.deviceIds);

            var path = string.Empty;
            long id = YitIdHelper.NextId();
            if (request.pid != null && request.pid > 0)
            {
                //var allList = await context.Groups.AsNoTracking().Where(w => !w.IsDelete).ToListAsync();
                //if (allList.Where(w => w.Id == request.pid).Count() == 0)
                //    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0011)]);
                //// 获取父级信息
                //var parentHierarchy = info.GetParentHierarchy(request.pid.Value, allList);

                //// 计算层级数：父级层级数 + 当前节点
                //int levelCount = parentHierarchy.Count + 2;
                //if (levelCount > 8)
                //    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0069)]);

                var parentGroup = await context.Groups.AsNoTracking().Where(w => w.Id == request.pid && !w.IsDelete).FirstOrDefaultAsync();
                if (parentGroup == null)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0011)]);

                var parentPath = parentGroup.Path;
                if (parentPath.Split('.').Count() > 7)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0069)]);

                path = $"{parentPath}.{id}";
            }
            else
            {
                path = $"{id}";
            }

            var info = new Groups(request.enterpriseInfoId, request.name, request.pid, request.Remark, request.userIds, request.deviceIds, path, id);
            var res = await repository.AddAsync(info);
            return res != null;
        }
    }
}
