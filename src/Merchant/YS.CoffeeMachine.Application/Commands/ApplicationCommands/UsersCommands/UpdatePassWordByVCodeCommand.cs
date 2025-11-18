using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.ApplicationCommands.UsersCommands
{
    /// <summary>
    /// 获取验证码
    /// </summary>
    public record UpdatePassWordByVCodeCommand(int vCode, string newPwd) : ICommand<bool>;
}