using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.RolesCommands
{
    public record P_CreateRoleCommand(string name, SysMenuTypeEnum sysMenuType,bool? hasSuperAdmin, int sort, string remark, List<long>? menuIds, RoleStatusEnum? RoleStatus = null) : ICommand<bool>;
}
