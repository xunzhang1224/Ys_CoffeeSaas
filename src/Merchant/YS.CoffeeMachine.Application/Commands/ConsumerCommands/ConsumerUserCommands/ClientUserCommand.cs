using YS.CoffeeMachine.Application.Dtos.ConsumerDtos;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.ConsumerCommands.ConsumerUserCommands
{
    /// <summary>
    /// 微信小程序登录
    /// </summary>
    /// <param name="code"></param>
    public record ClientUserWxLoginCommand(string code) : ICommand<ClientLoginResponseDto>;

    /// <summary>
    /// 消费者端登录
    /// </summary>
    /// <param name="account"></param>
    /// <param name="password"></param>
    public record ClientUserLoginCommand(string account, string password) : ICommand<ClientLoginResponseDto>;

    /// <summary>
    /// 消费者端注册
    /// </summary>
    /// <param name="account"></param>
    /// <param name="nickName"></param>
    /// <param name="password"></param>
    /// <param name="phone"></param>
    /// <param name="email"></param>
    /// <param name="sex"></param>
    public record ClientUserRegisterCommand(string account, string nickName, string password, string phone, string email, int? sex) : ICommand<bool>;

    /// <summary>
    /// 登录刷新
    /// </summary>
    /// <param name="refreshToken"></param>
    public record ClientUserRefreshCommand(string refreshToken) : ICommand<ClientLoginResponseDto>;
}