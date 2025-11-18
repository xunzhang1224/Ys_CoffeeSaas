using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.UsersCommands
{
    public record P_CreateUserCommand(string account, string password, string nickName, string areaCode, string phone, string email, AccountTypeEnum accountType, string remark, List<long> roleIds) : ICommand<bool>;
}
