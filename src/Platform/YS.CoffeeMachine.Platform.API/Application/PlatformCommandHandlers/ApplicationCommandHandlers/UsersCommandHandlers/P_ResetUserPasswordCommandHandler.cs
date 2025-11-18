using YS.CoffeeMachine.Application.Dtos.EmailDtos;
using YS.CoffeeMachine.Application.IServices;
using YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.UsersCommands;
using YS.CoffeeMachine.Domain.IPlatformRepositories.ApplicationUsersIRepositories;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.ApplicationCommandHandlers.UsersCommandHandlers
{
    /// <summary>
    /// 修改用户密码
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="passwordHasher"></param>
    public class P_ResetUserPasswordCommandHandler(IPApplicationUserRepository repository, IPasswordHasher passwordHasher, IEmailServiceProvider emailServiceProvider) : ICommandHandler<P_ResetUserPasswordCommand, bool>
    {
        /// <summary>
        /// 修改用户密码
        /// </summary>
        public async Task<bool> Handle(P_ResetUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var info = await repository.GetByIdAsync(request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

            //创建随机密码
            var password = passwordHasher.CreateRandomPassword();

            //发送邮件
            var mailRes = await emailServiceProvider.SendEmailSingleAsync(new EmailObject() { ToEmail = info.Email, MessageBody = string.Format(L.Text[nameof(ErrorCodeEnum.D0040)], password), Subject = L.Text[nameof(ErrorCodeEnum.D0041)] });

            info.UpdatePassWord(passwordHasher.HashPassword(password));
            var res = await repository.UpdateAsync(info);
            return res != null;
        }
    }
}