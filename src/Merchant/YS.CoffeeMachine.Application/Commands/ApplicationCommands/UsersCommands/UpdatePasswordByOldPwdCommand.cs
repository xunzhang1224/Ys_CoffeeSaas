using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.ApplicationCommands.UsersCommands
{
    /// <summary>
    /// 通过旧密码更新密码
    /// </summary>
    /// <param name="oldPwd"></param>
    /// <param name="newPwd"></param>
    public record UpdatePasswordByOldPwdCommand(string oldPwd, string newPwd) : ICommand<bool>;
}
