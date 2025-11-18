using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.ApplicationCommands.EnterpriseCommands
{
    public record CreateEnterpriseInfoCommand(string name, long enterpriseTypeId, long pid, string? remark, List<long>? userIds, List<long>? roleIds,long? areaRelationId) : ICommand<bool>;
}
