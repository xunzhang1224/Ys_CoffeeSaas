using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Commands.DevicesCommands.GroupsCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Domain.IPlatformRepositories.DevicesIRepositories;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.DevicesCommandHandlers.GroupsCommandHandler
{
    /// <summary>
    /// 新增分组
    /// </summary>
    /// <param name="repository"></param>
    public class CreateGroupsCommandHandler(IPGroupsRepository repository, CoffeeMachinePlatformDbContext context) : ICommandHandler<CreateGroupsCommand, bool>
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
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.C0008)]);

            var info = new Groups(request.enterpriseInfoId, request.name, request.pid, request.Remark, request.userIds, request.deviceIds);
            if (request.pid != null && request.pid > 0)
            {
                var allList = await context.Groups.AsNoTracking().Where(w => !w.IsDelete).ToListAsync();
                if (allList.Where(w => w.Id == request.pid).Count() == 0)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0011)]);
                // 获取父级信息
                var parentHierarchy = info.GetParentHierarchy(request.pid.Value, allList);

                // 计算层级数：父级层级数 + 当前节点
                int levelCount = parentHierarchy.Count + 2;
                if (levelCount > 8)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0069)]);
            }
            var res = await repository.AddAsync(info);
            return res != null;
        }
    }
}
