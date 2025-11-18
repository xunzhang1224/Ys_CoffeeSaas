using YS.CoffeeMachine.Application.Commands.ApplicationCommands.UsersCommands;
using YS.CoffeeMachine.Application.Dtos.EmailDtos;
using YS.CoffeeMachine.Application.IServices;
using YS.CoffeeMachine.Domain.IPlatformRepositories.ApplicationUsersIRepositories;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.ApplicationCommandHandlers.UsersCommandHandlers
{
    /// <summary>
    /// 重置用户密码
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="passwordHasher"></param>
    /// <param name="emailServiceProvider"></param>
    public class ResetUserPasswordCommandHandler(IPApplicationUserRepository repository, IPasswordHasher passwordHasher, IEmailServiceProvider emailServiceProvider) : ICommandHandler<ResetUserPasswordCommand, bool>
    {
        /// <summary>
        /// 重置用户密码
        /// </summary>
        public async Task<bool> Handle(ResetUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var info = await repository.GetByIdAsync(request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            if (info.IsDefault)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0039)]);

            //创建随机密码
            var password = passwordHasher.CreateRandomPassword();

            //发送邮件
            var mailRes = await emailServiceProvider.SendEmailSingleAsync(new EmailObject() { ToEmail = info.Email, MessageBody = string.Format(L.Text[nameof(ErrorCodeEnum.D0040)], password), Subject = L.Text[nameof(ErrorCodeEnum.D0041)] });

            //更新密码
            info.UpdatePassWord(passwordHasher.HashPassword(password));
            var res = repository.Update(info);
            return res != null;
        }
    }
}
