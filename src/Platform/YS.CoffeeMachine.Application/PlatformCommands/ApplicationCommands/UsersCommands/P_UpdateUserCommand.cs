using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.UsersCommands
{
    public record P_UpdateUserCommand(long id, AccountTypeEnum accountType, string areaCode, string phone, string remark, List<long>? roleIds) : ICommand<bool>;
}
