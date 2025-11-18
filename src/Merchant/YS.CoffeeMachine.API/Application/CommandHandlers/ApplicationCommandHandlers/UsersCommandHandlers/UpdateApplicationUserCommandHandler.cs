using FreeRedis;
using Newtonsoft.Json;
using YS.CoffeeMachine.Application.Commands.ApplicationCommands.UsersCommands;
using YS.CoffeeMachine.Application.IServices;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.IRepositories.ApplicationUsersIRepositories;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.ApplicationCommandHandlers.UsersCommandHandlers
{
    /// <summary>
    /// 编辑用户信息
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="passwordHasher"></param>
    /// <param name="_redisClient"></param>
    /// <param name="aliyunSmsService"></param>
    public class UpdateApplicationUserCommandHandler(IApplicationUserRepository repository, IPasswordHasher passwordHasher, IRedisClient _redisClient, IAliyunSmsService aliyunSmsService) : ICommandHandler<UpdateApplicationUserCommand, bool>
    {
        private static string GetBasketUserKey(long userId) => $"/UserMenus/Merchant/Menu{userId}";

        /// <summary>
        /// 编辑用户信息
        /// </summary>
        public async Task<bool> Handle(UpdateApplicationUserCommand request, CancellationToken cancellationToken)
        {
            //验证默认用户电话邮箱是否存在
            //var isExist = await repository.AnyAsync(x => ((!string.IsNullOrWhiteSpace(request.phone) && x.Phone == request.phone) || x.Email == request.email) && x.Id != request.id);
            //if (isExist)
            //    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0025)]);
            var info = await repository.GetByIdAsync(request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

            //修改用户信息
            info.Update(request.account, request.nickName, request.areaCode, request.phone, request.email, request.remark);
            //修改用户状态
            if (request.status != null)
            {
                if (info.IsDefault && request.status == UserStatusEnum.Disable)
                    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0042)]);
                info.UpdateStatus(request.status.Value);
            }
            //修改账号类型
            //if (request.accountType != null)
            //{
            //    if (info.IsDefault)
            //        throw ExceptionHelper.AppFriendly("默认管理员不允许修改账号类型");
            //    info.UpdateAccountType(request.accountType.Value);
            //}
            //修改所属系统类型
            //if (request.sysMenuType != null)
            //    info.UpdateSysMenuType(request.sysMenuType.Value);

            //修改密码
            if (request.newPassword != null && !string.IsNullOrWhiteSpace(info.Phone))
            {
                info.UpdatePassWord(passwordHasher.HashPassword(request.newPassword));

                // 发送密码修改短信通知
                string templateParamJson = JsonConvert.SerializeObject(new
                {
                    user_nick = info.NickName,
                    password = request.newPassword
                });
                await aliyunSmsService.SendSmsAsync(info.Phone, SmsConst.OperatorResetPassword, templateParamJson);
            }
            //修改用户角色
            if (request.roleIds != null && !request.roleIds.OrderBy(o => o).SequenceEqual(info.ApplicationUserRoles.Select(s => s.RoleId).OrderBy(o => o)))
            {
                //删除用户菜单缓存
                await _redisClient.DelAsync(GetBasketUserKey(info.Id));
                info.UpdateUserRoleIds(request.roleIds);
            }
            //提交数据
            var res = await repository.UpdateAsync(info);
            return res != null;
        }
    }
}
