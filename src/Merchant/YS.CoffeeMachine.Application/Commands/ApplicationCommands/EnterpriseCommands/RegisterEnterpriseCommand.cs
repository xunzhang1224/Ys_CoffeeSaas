using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.ApplicationCommands.EnterpriseCommands
{
    /// <summary>
    /// 发送邮箱验证密码
    /// </summary>
    /// <param name="email"></param>
    public record SendVcodeCommand(string email) : ICommand<bool>;

    /// <summary>
    /// 发送短信验证密码
    /// </summary>
    /// <param name="phone"></param>
    public record SendSMSVcodeCommand(string phone) : ICommand<bool>;

    /// <summary>
    /// 企业注册
    /// </summary>
    public record RegisterEnterpriseCommand(RegisterUserInfoInput input) : ICommand<LoginResponseDto>;

    /// <summary>
    /// 设置企业组织类型
    /// </summary>
    /// <param name="organizationType"></param>
    public record SetOrganizationTypeCommand(EnterpriseOrganizationTypeEnum organizationType) : ICommand<bool>;

    /// <summary>
    /// 更新企业资质信息
    /// </summary>
    /// <param name="qualificationInfo"></param>
    public record UpdateEnterpriseQualificationInfoCommand(EnterpriseQualificationInfoDto qualificationInfo) : ICommand<bool>;
}