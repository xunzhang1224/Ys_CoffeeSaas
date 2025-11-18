using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using YS.CoffeeMachine.Application.Dtos.EmailDtos;
using YS.CoffeeMachine.Application.IServices;
using YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.EnterpriseCommands;
using YS.CoffeeMachine.Domain.IPlatformRepositories.ApplicationUsersIRepositories;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.ApplicationCommandHandlers.EnterpriseCommandHandlers
{
    /// <summary>
    /// 重置企业默认管理员密码
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="context"></param>
    /// <param name="passwordHasher"></param>
    public class ResetAdministratorPasswordCommandHandler(IPEnterpriseInfoRepository repository, CoffeeMachinePlatformDbContext context,
        IPasswordHasher passwordHasher, IEmailServiceProvider emailServiceProvider,
        IAliyunSmsService aliyunSmsService, UserHttpContext userHttp) : ICommandHandler<ResetAdministratorPasswordCommand, bool>
    {
        /// <summary>
        /// 重置企业默认管理员密码
        /// </summary>
        public async Task<bool> Handle(ResetAdministratorPasswordCommand request, CancellationToken cancellationToken)
        {
            //获取企业信息
            var info = await repository.GetAsync(request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            //获取默认用户
            var defaultUser = await context.ApplicationUser.FirstAsync(w => w.EnterpriseId == request.id && w.IsDefault);
            if (defaultUser == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0078)]);

            //创建随机密码
            var password = passwordHasher.CreateRandomPassword();

            var aa = string.Format(L.Text[nameof(ErrorCodeEnum.D0040)], password);

            //更新密码
            defaultUser.UpdatePassWord(passwordHasher.HashPassword(password));
            var res = repository.Update(info);

            // 发送邮件
            var mailRes = await emailServiceProvider.SendEmailSingleAsync(new EmailObject() { ToEmail = defaultUser.Email, MessageBody = string.Format(L.Text[nameof(ErrorCodeEnum.D0040)], password), Subject = L.Text[nameof(ErrorCodeEnum.D0041)] });

            // 发送短信
            string templateParamJson = JsonConvert.SerializeObject(new
            {
                user_nick = defaultUser.Account,
                password = password
            });

            // 发送验证码
            if (!string.IsNullOrWhiteSpace(defaultUser.Phone))
                await aliyunSmsService.SendSmsAsync(defaultUser.Phone, SmsConst.PlatformResetPassword, templateParamJson);
            return res != null;
        }
    }
}