using FreeRedis;
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
    /// 编辑用户信息
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="_redisClient"></param>
    public class P_UpdateUserCommandHandler(IPApplicationUserRepository repository, IRedisClient _redisClient) : ICommandHandler<P_UpdateUserCommand, bool>
    {
        private static string GetBasketUserKey(long userId) => $"/UserMenus/Platform/Menu{userId}";

        /// <summary>
        /// 编辑用户信息
        /// </summary>
        public async Task<bool> Handle(P_UpdateUserCommand request, CancellationToken cancellationToken)
        {
            //验证默认用户电话邮箱是否存在
            //var isExist = await repository.AnyAsync(x => (!string.IsNullOrWhiteSpace(x.Phone) && x.Phone == request.phone) && x.Id != request.id);
            //if (isExist)
            //    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0025)]);
            var info = await repository.GetByIdAsync(request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0029)]);

            // 修改用户信息
            info.Update(info.Account, info.NickName, request.areaCode, request.phone, info.Email, request.remark);
            if (request.roleIds != null && !request.roleIds.OrderBy(o => o).SequenceEqual(info.ApplicationUserRoles.Select(s => s.RoleId).OrderBy(o => o)))
            {
                //删除用户菜单缓存
                await _redisClient.DelAsync(GetBasketUserKey(info.Id));
                info.UpdateUserRoleIds(request.roleIds);
            }

            // 更新账号类型
            info.UpdateAccountType(request.accountType);
            if (request.accountType == AccountTypeEnum.SuperAdmin)
            {
                await _redisClient.DelAsync(GetBasketUserKey(info.Id));
                info.CelarUserRoles();
            }

            // 提交数据
            var res = await repository.UpdateAsync(info);
            return res != null;
        }
    }
}