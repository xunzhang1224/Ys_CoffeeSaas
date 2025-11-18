using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YS.CoffeeMachine.Application.Dtos.VerificationCodeDtos;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.ApplicationCommands.UsersCommands
{
    /// <summary>
    /// 账号密码登录
    /// </summary>
    /// <param name="account"></param>
    /// <param name="password"></param>
    public record LoginCommand(string account, string password) : ICommand<LoginResponseDto>;

    /// <summary>
    /// 发送登录短信验证码
    /// </summary>
    /// <param name="phone"></param>
    public record SendSMSLoginCodeCommand(string phone) : ICommand<SendVerificationCodeResponseDto>;

    /// <summary>
    /// 短信验证码登录
    /// </summary>
    /// <param name="phone"></param>
    /// <param name="vCode"></param>
    public record SmsLoginCommand(string phone, string vCode) : ICommand<LoginResponseDto>;

    /// <summary>
    /// 发送邮箱验证码
    /// </summary>
    /// <param name="email"></param>
    public record SendEmailLoginCodeCommand(string email) : ICommand<SendVerificationCodeResponseDto>;

    /// <summary>
    /// 邮箱登录
    /// </summary>
    /// <param name="email"></param>
    /// <param name="eCode"></param>
    public record EmailLoginCommand(string email, string eCode) : ICommand<LoginResponseDto>;

    /// <summary>
    /// 切换企业
    /// </summary>
    /// <param name="enterpriseId"></param>
    public record ChangeEnterpriseByIdCommand(long enterpriseId) : ICommand<LoginResponseDto>;

    /// <summary>
    /// 切换账号
    /// </summary>
    /// <param name="id"></param>
    public record ChangeAccountBuUserIdCommand(long id) : ICommand<LoginResponseDto>;
}