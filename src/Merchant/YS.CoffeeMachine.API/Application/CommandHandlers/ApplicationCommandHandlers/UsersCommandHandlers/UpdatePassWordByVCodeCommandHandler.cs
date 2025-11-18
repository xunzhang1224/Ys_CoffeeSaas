using FreeRedis;
using YS.CoffeeMachine.API.Extensions;
using YS.CoffeeMachine.Application.Commands.ApplicationCommands.UsersCommands;
using YS.CoffeeMachine.Application.Dtos.ConstantModels;
using YS.CoffeeMachine.Application.Dtos.VerificationCodeDtos;
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
    /// 通过验证码更新密码
    /// </summary>
    /// <param name="context"></param>
    /// <param name="_redisClient"></param>
    /// <param name="_user"></param>
    /// <param name="passwordHasher"></param>
    public class UpdatePassWordByVCodeCommandHandler(CoffeeMachineDbContext context, IRedisClient _redisClient, UserHttpContext _user, IPasswordHasher passwordHasher) : ICommandHandler<UpdatePassWordByVCodeCommand, bool>
    {
        /// <summary>
        /// 通过验证码更新密码
        /// </summary>
        public async Task<bool> Handle(UpdatePassWordByVCodeCommand request, CancellationToken cancellationToken)
        {
            if (_user == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0044)]);

            //获取验证码并校验
            //var vCode = await _redisClient.GetAsync<EmailVCodeDto>(RedisKeyConstants.GetUserVCodeKey(_user.UserId));
            //if (string.IsNullOrEmpty(vCode.Code))
            //    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0034)]);
            //if (vCode.Code != request.vCode.ToString() || vCode.Status == 1)
            //    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0035)]);

            var vCode = await _redisClient.GetAsync(RedisKeyConstants.GetUserVCodeKey(_user.UserId));
            if (string.IsNullOrEmpty(vCode))
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0034)]);
            if (vCode != request.vCode.ToString())
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0035)]);
            await _redisClient.DelAsync(RedisKeyConstants.GetUserVCodeKey(_user.UserId));

            //校验通过修改密码
            var user = await context.ApplicationUser.FindAsync(_user.UserId);
            if (user == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0029)]);
            //修改密码
            user.UpdatePassWord(passwordHasher.HashPassword(request.newPwd));
            context.ApplicationUser.Update(user);

            return true;
        }
    }
}
