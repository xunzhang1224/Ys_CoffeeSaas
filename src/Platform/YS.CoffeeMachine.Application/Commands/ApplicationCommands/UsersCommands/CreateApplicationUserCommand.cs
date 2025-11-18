using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.ApplicationCommands.UsersCommands
{
    public record CreateApplicationUserCommand(long enterpriseId, string account, string password, string nickName, string areaCode, string phone, string email, string remark, List<long>? roleIds, SysMenuTypeEnum sysMenuType = SysMenuTypeEnum.Merchant) : ICommand<bool>;
}
