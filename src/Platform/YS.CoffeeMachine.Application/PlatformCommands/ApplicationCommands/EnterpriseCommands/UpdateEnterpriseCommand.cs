using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.EnterpriseCommands
{
    public record UpdateEnterpriseCommand(long id, string enterpriseName, /*string account, string nickName, string areaCode, string phone, string emial,*/ string? remark, long? areaRelationId = null) : ICommand<bool>;

    public record UpdateRegistrationAuditCommand(long id, string? remark, RegistrationProgress registrationProgress) : ICommand<bool>;
}
