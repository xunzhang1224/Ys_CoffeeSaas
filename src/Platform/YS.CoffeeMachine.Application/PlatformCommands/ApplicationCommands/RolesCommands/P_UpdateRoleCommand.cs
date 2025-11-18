using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.RolesCommands
{
    public record P_UpdateRoleCommand(long id, string name, int sort, bool? hasSuperAdmin, string remark, RoleStatusEnum? roleStatus, SysMenuTypeEnum? sysMenuType, List<long>? menuIds, List<long>? halfMenuIds) : ICommand<bool>;
}
