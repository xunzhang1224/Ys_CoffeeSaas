using YS.CoffeeMachine.Application.Commands.ApplicationCommands.UsersCommands;
using YS.CoffeeMachine.Application.IServices;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.IPlatformRepositories.ApplicationUsersIRepositories;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.CommandHandlers.ApplicationCommandHandlers.UsersCommandHandlers
{
    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="passwordHasher"></param>
    public class CreateApplicationUserCommandHandler(IPApplicationUserRepository repository, IPasswordHasher passwordHasher) : ICommandHandler<CreateApplicationUserCommand, bool>
    {
        /// <summary>
        /// 创建用户
        /// </summary>
        public async Task<bool> Handle(CreateApplicationUserCommand request, CancellationToken cancellationToken)
        {
            //验证默认用户电话邮箱是否存在
            //var isExist = await repository.AnyAsync(x => (x.AreaCode + x.Phone == request.areaCode + request.phone && !string.IsNullOrWhiteSpace(request.phone)) || x.Email == request.email);
            //if (isExist)
            //    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0025)]);
            var password = request.password;
            if (string.IsNullOrWhiteSpace(request.password))
            {
                password = passwordHasher.CreateRandomPassword();
            }
            return await repository.AddAndBindAsync(request.enterpriseId, request.account, passwordHasher.HashPassword(password), request.nickName, request.areaCode, request.phone, request.email, UserStatusEnum.Enable, AccountTypeEnum.NormalUser, request.sysMenuType, request.remark, request.roleIds);
        }
    }
}