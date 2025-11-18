using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.ApplicationCommands.UsersCommands
{
    public record UpdateApplicationUserCommand(long id, string account, string nickName, string areaCode, string phone, string email, string remark, UserStatusEnum? status = null, string? newPassword = null, List<long>? roleIds = null) : ICommand<bool>;
}
