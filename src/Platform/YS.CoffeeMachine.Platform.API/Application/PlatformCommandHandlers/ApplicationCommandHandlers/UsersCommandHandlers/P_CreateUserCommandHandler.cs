using YS.CoffeeMachine.Application.Dtos.EmailDtos;
using YS.CoffeeMachine.Application.IServices;
using YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.UsersCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.IPlatformRepositories.ApplicationUsersIRepositories;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.ApplicationCommandHandlers.UsersCommandHandlers
{
    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="passwordHasher"></param>
    public class P_CreateUserCommandHandler(IPApplicationUserRepository repository, IPasswordHasher passwordHasher, IEmailServiceProvider emailServiceProvider) : ICommandHandler<P_CreateUserCommand, bool>
    {
        /// <summary>
        /// 创建用户
        /// </summary>
        public async Task<bool> Handle(P_CreateUserCommand request, CancellationToken cancellationToken)
        {
            // 验证默认用户电话邮箱是否存在
            //var isExist = await repository.AnyAsync(x => ((x.AreaCode + x.Phone == request.areaCode + request.phone && !string.IsNullOrWhiteSpace(request.phone)) || x.Email == request.email) && x.SysMenuType == SysMenuTypeEnum.Platform);
            //if (isExist)
            //    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0025)]);

            var password = request.password;
            if (string.IsNullOrWhiteSpace(request.password))
            {
                password = passwordHasher.CreateRandomPassword();

                // 发送邮件
                // TODO:发送邮件多语言配置
                var mailRes = await emailServiceProvider.SendEmailSingleAsync(new EmailObject() { ToEmail = request.email, MessageBody = string.Format(L.Text[nameof(ErrorCodeEnum.D0026)], password), Subject = L.Text[nameof(ErrorCodeEnum.D0027)] });
            }
            var user = new ApplicationUser(0, request.account, passwordHasher.HashPassword(password), request.nickName, request.areaCode, request.phone, request.email, UserStatusEnum.Enable, request.accountType, SysMenuTypeEnum.Platform, request.remark, request.roleIds);
            return await repository.AddAsync(user) != null;
        }
    }
}
