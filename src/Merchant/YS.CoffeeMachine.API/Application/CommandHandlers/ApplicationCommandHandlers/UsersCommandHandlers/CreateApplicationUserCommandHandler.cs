using Newtonsoft.Json;
using YS.CoffeeMachine.Application.Commands.ApplicationCommands.UsersCommands;
using YS.CoffeeMachine.Application.Dtos.EmailDtos;
using YS.CoffeeMachine.Application.IServices;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.IRepositories.ApplicationUsersIRepositories;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.API.Application.CommandHandlers.ApplicationCommandHandlers.UsersCommandHandlers
{
    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="passwordHasher"></param>
    /// <param name="emailServiceProvider"></param>
    public class CreateApplicationUserCommandHandler(IApplicationUserRepository repository, IPasswordHasher passwordHasher, IEmailServiceProvider emailServiceProvider, IAliyunSmsService aliyunSmsService, UserHttpContext userHttp) : ICommandHandler<CreateApplicationUserCommand, bool>
    {
        /// <summary>
        /// 创建用户
        /// </summary>
        public async Task<bool> Handle(CreateApplicationUserCommand request, CancellationToken cancellationToken)
        {
            // 登录账号不能重复
            var isExistAccount = await repository.AnyAsync(x => x.Account == request.account);
            if (isExistAccount)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0083)]);

            // 验证默认用户电话邮箱是否存在
            //var isExist = await repository.AnyAsync(x => ((x.AreaCode + x.Phone == request.areaCode + request.phone && !string.IsNullOrWhiteSpace(request.phone)) || x.Email == request.email) && x.SysMenuType == SysMenuTypeEnum.Merchant);
            //if (isExist)
            //    throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0025)]);
            var password = request.password;
            if (string.IsNullOrWhiteSpace(request.password))
            {
                password = passwordHasher.CreateRandomPassword();
            }

            // 发送邮件
            // TODO:发送邮件多语言配置
            var mailRes = await emailServiceProvider.SendEmailSingleAsync(new EmailObject() { ToEmail = request.email, MessageBody = string.Format(L.Text[nameof(ErrorCodeEnum.D0026)], password), Subject = L.Text[nameof(ErrorCodeEnum.D0027)] });

            if (!string.IsNullOrWhiteSpace(request.phone))
            {
                // 发送密码修改短信通知
                string templateParamJson = JsonConvert.SerializeObject(new
                {
                    orgname = userHttp.NickName,
                    user_nick = request.account,
                    password = password
                });
                await aliyunSmsService.SendSmsAsync(request.phone, SmsConst.CreateSuperAdminWhenAdd, templateParamJson);
            }

            return await repository.AddAndBindAsync(request.enterpriseId, request.account, passwordHasher.HashPassword(password), request.nickName, request.areaCode, request.phone, request.email, UserStatusEnum.Enable, AccountTypeEnum.NormalUser, request.sysMenuType, request.remark, request.roleIds);
        }
    }
}