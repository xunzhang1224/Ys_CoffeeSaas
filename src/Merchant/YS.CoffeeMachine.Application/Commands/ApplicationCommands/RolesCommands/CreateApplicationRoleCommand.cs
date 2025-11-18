using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.ApplicationCommands.RolesCommands
{
    public record CreateApplicationRoleCommand(long enterpriseId, string name, RoleStatusEnum roleStatus, bool? hasSuperAdmin, int sort, string remark, List<long>? menuIds) : ICommand<bool>;
}
