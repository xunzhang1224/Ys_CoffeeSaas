using FreeRedis;
using YS.CoffeeMachine.API.Extensions;
using YS.CoffeeMachine.Application.Commands.VerificationCodeCommands;
using YS.CoffeeMachine.Application.Dtos.ConstantModels;
using YS.CoffeeMachine.Application.Dtos.EmailDtos;
using YS.CoffeeMachine.Application.Dtos.VerificationCodeDtos;
using YS.CoffeeMachine.Application.IServices;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.VerificationCodeCommandHandlers
{
    /// <summary>
    /// 发送验证码
    /// </summary>
    /// <param name="_redisClient"></param>
    /// <param name="_user"></param>
    /// <param name="emailServiceProvider"></param>
    public class SendVerificationCodeCommandHandler(IRedisClient _redisClient, UserHttpContext _user, IEmailServiceProvider emailServiceProvider) : ICommandHandler<SendVerificationCodeCommand, SendVerificationCodeResponseDto>
    {
        /// <summary>
        /// 发送验证码
        /// </summary>
        public async Task<SendVerificationCodeResponseDto> Handle(SendVerificationCodeCommand request, CancellationToken cancellationToken)
        {
            if (_user == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0044)]);

            //生成验证码
            Random random = new Random();
            var vCode = random.Next(100000, 999999).ToString();

            //缓存验证码，有效期5分钟
            await _redisClient.SetAsync(RedisKeyConstants.GetUserVCodeKey(_user.UserId), vCode, 5 * 60);

            //发送验证码
            var res = await emailServiceProvider.SendEmailSingleAsync(new EmailObject() { ToEmail = _user.Email, MessageBody = string.Format(L.Text[nameof(ErrorCodeEnum.D0032)], vCode), Subject = L.Text[nameof(ErrorCodeEnum.D0033)] });

            return new SendVerificationCodeResponseDto() { IsSuccess = res.success, Message = res.response, ExpireTime = 5 };
        }
    }
}