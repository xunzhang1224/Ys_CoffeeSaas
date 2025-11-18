using YS.CoffeeMachine.Application.Commands.ApplicationCommands.RolesCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.IPlatformRepositories.ApplicationUsersIRepositories;
using YS.CoffeeMachine.Infrastructure;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.ApplicationCommandHandlers.RolesCommandHandlers
{
    /// <summary>
    /// 创建角色命令处理程序
    /// </summary>
    /// <param name="repository"></param>
    public class CreateApplicationRoleCommandHandler(IPApplicationRoleRepository repository, CoffeeMachinePlatformDbContext context) : ICommandHandler<CreateApplicationRoleCommand, bool>
    {
        /// <summary>
        /// 创建角色命令处理程序
        /// </summary>
        public async Task<bool> Handle(CreateApplicationRoleCommand request, CancellationToken cancellationToken)
        {
            var exist = context.ApplicationRole.AsQueryable().Any(x => x.Name == request.name);
            if (exist)
                throw ExceptionHelper.AppFriendly("角色名称重复");

            //var info = new ApplicationRole(request.enterpriseId, request.name, request.roleStatus, request.sort, request.remark, request.menuIds);
            //创建角色并绑定企业
            return await repository.AddAndBindAsync(request.name, request.roleStatus, SysMenuTypeEnum.Merchant, request.hasSuperAdmin, request.sort, request.remark, request.menuIds, request.enterpriseId);

        }
    }
}
