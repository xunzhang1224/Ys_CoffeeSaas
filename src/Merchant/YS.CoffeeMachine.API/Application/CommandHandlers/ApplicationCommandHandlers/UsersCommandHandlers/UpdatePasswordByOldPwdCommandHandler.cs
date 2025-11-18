using Microsoft.AspNetCore.Identity;
using YS.CoffeeMachine.Application.Commands.ApplicationCommands.UsersCommands;
using YS.CoffeeMachine.Application.IServices;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.ApplicationCommandHandlers.UsersCommandHandlers
{
    /// <summary>
    /// 通过旧密码更新密码
    /// </summary>
    public class UpdatePasswordByOldPwdCommandHandler(CoffeeMachineDbContext context, IPasswordHasher passwordHasher, UserHttpContext _user) : ICommandHandler<UpdatePasswordByOldPwdCommand, bool>
    {
        /// <summary>
        /// 通过旧密码更新密码
        /// </summary>
        public async Task<bool> Handle(UpdatePasswordByOldPwdCommand request, CancellationToken cancellationToken)
        {
            //获取用户信息
            var user = await context.ApplicationUser.FindAsync(_user.UserId);
            if (user == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0029)]);

            //校验旧密码
            var result = passwordHasher.VerifyHashedPassword(user.Password, request.oldPwd);
            if (result == PasswordVerificationResult.Failed)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0043)]);

            //修改密码
            user.UpdatePassWord(passwordHasher.HashPassword(request.newPwd));
            context.ApplicationUser.Update(user);

            return true;
        }
    }
}