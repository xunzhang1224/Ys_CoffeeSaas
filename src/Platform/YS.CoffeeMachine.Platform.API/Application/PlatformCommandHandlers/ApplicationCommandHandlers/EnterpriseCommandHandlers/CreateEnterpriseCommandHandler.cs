using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using YS.CoffeeMachine.Application.Dtos.EmailDtos;
using YS.CoffeeMachine.Application.IServices;
using YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.EnterpriseCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.Shared.Const;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.ApplicationCommandHandlers.EnterpriseCommandHandlers
{
    /// <summary>
    /// 平台端创建企业信息命令处理程序
    /// </summary>
    /// <param name="context"></param>
    /// <param name="passwordHasher"></param>
    /// <param name="emailServiceProvider"></param>
    /// <param name="aliyunSmsService"></param>
    /// <param name="userHttp"></param>
    public class CreateEnterpriseCommandHandler(CoffeeMachinePlatformDbContext context, IPasswordHasher passwordHasher, IEmailServiceProvider emailServiceProvider, IAliyunSmsService aliyunSmsService, UserHttpContext userHttp) : ICommandHandler<CreateEnterpriseInfoCommand, bool>
    {
        /// <summary>
        /// 平台端创建企业信息命令处理程序
        /// </summary>
        public async Task<bool> Handle(CreateEnterpriseInfoCommand request, CancellationToken cancellationToken)
        {
            var result = false;
            // 验证默认用户电话邮箱是否存在
            //var isExist = await context.ApplicationUser.AnyAsync(x => (!string.IsNullOrWhiteSpace(x.Phone) && x.Phone == request.phone) || x.Email == request.emial);
            // Todo:判断账号是否重复即可，后续邮箱和手机是可以多绑定的且可切换企业
            var isExist = await context.ApplicationUser.AnyAsync(x => x.Account == request.account);

            if (isExist)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0083)]); //D0025

            // 创建企业信息
            var info = new EnterpriseInfo(request.enterpriseName, request.enterpriseTypeId, null, request.remark, null, null, null, null, null, request.areaRelationId);
            // 插入默认企业信息
            //info.InsertDefaultEnterprise(request.account, request.nickName, request.phone, request.emial);

            // 获取所有菜单
            var menus = await context.ApplicationMenu.AsNoTracking().ToListAsync();
            // 获取所有菜单Ids
            var allMenusIds = menus.Where(w => w.SysMenuType == SysMenuTypeEnum.Merchant).Select(s => s.Id).ToList();
            // 企业绑定菜单Ids
            info.UpdateMenuIds(allMenusIds);

            // 获取所有菜单Ids
            var allH5MenusIds = menus.Where(w => w.SysMenuType == SysMenuTypeEnum.H5).Select(s => s.Id).ToList();
            // 企业绑定菜单Ids
            info.UpdateH5MenuIds(allH5MenusIds);

            // 持久化企业信息
            var res = await context.EnterpriseInfo.AddAsync(info);

            // 添加用户信息
            var password = passwordHasher.CreateRandomPassword();

            // 默认用户绑定角色
            //var defaultRoles = await context.ApplicationRole.Where(x => x.IsDefault && x.SysMenuType == SysMenuTypeEnum.Merchant).ToListAsync();
            var roleIds = new List<long>();
            //if (defaultRoles != null && defaultRoles.Count > 0)
            //    roleIds.AddRange(defaultRoles.Select(x => x.Id).ToList());

            // 添加企业默认用户
            var applicationUser = new ApplicationUser(info.Id, request.account, passwordHasher.HashPassword(password), request.nickName, request.areaCode,
                request.phone, request.emial, UserStatusEnum.Enable, AccountTypeEnum.SuperAdmin, SysMenuTypeEnum.Merchant, "默认账号", roleIds);
            applicationUser.SetDefault();

            //持久化用户信息
            var userRes = await context.ApplicationUser.AddAsync(applicationUser);

            // 返回结果
            result = await context.SaveChangesAsync() > 0;

            if (res.Entity != null && userRes.Entity != null)
            {
                // 绑定企业与用户关系
                res.Entity.UpdateEnterpriseUsers(new List<long>() { userRes.Entity.Id });
                context.Update(info);
                var updateRes = await context.SaveChangesAsync();
                result = updateRes > 0;
            }

            // 发送邮件
            var mailRes = await emailServiceProvider.SendEmailSingleAsync(new EmailObject() { ToEmail = request.emial, MessageBody = string.Format(L.Text[nameof(ErrorCodeEnum.D0026)], password), Subject = L.Text[nameof(ErrorCodeEnum.D0027)] });

            // 发送短信
            string templateParamJson = JsonConvert.SerializeObject(new
            {
                orgname = userHttp.NickName,
                user_nick = request.account,
                password = password
            });

            // 发送验证码
            await aliyunSmsService.SendSmsAsync(request.phone, SmsConst.CreateSuperAdminWhenAdd, templateParamJson);

            return result;
        }
    }
}