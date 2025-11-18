using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.ApplicationCommands.EnterpriseCommands
{
    public record UpdateEnterpriseInfoCommand(long id, string name, long? enterpriseTypeId, long? pid, List<long>? userIds, List<long>? roleIds, string? remark, long? areaRelationId = null) : ICommand<bool>;
}