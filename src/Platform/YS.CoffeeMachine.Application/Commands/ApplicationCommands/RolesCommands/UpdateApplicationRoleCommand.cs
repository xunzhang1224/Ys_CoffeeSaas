using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.ApplicationCommands.RolesCommands
{
    public record UpdateApplicationRoleCommand(long id, string name, int sort, string remark, RoleStatusEnum? roleStatus, SysMenuTypeEnum? sysMenuType, List<long>? menuIds) : ICommand<bool>;
}
