using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.EnterpriseCommands
{
    public record CreateEnterpriseInfoCommand(string enterpriseName, long enterpriseTypeId, string account, string nickName, string areaCode, string phone, string emial, string? remark,long? areaRelationId) : ICommand<bool>;
}
