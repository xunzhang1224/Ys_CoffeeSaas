using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using YS.CoffeeMachine.Application.Dtos.EmailDtos;
using YS.CoffeeMachine.Application.IServices;
using YS.CoffeeMachine.Application.PlatformCommands.ApplicationCommands.EnterpriseCommands;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Events;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformCommandHandlers.ApplicationCommandHandlers.EnterpriseCommandHandlers
{
    /// <summary>
    /// 编辑企业
    /// </summary>
    /// <param name="context"></param>
    public class UpdateEnterpriseCommandHandler(CoffeeMachinePlatformDbContext context) : ICommandHandler<UpdateEnterpriseCommand, bool>
    {
        /// <summary>
        /// 编辑企业
        /// </summary>
        public async Task<bool> Handle(UpdateEnterpriseCommand request, CancellationToken cancellationToken)
        {
            //获取企业信息
            var info = await context.EnterpriseInfo.Include(i => i.Users).ThenInclude(ti => ti.User).FirstOrDefaultAsync(x => x.Id == request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);

            //获取默认用户
            //var defaultUser = info.Users.Select(s => s.User).FirstOrDefault(w => w.IsDefault && w.AccountType == AccountTypeEnum.SuperAdmin);
            //if (defaultUser == null)
            //    throw ExceptionHelper.AppFriendly("数据异常默认管理员不存在");

            ////更新默认用户信息
            //defaultUser.Update(request.account, request.nickName, request.areaCode, request.phone, request.emial, defaultUser.Remark);

            //验证默认用户电话邮箱是否存在
            //var isExist = await context.ApplicationUser.AnyAsync(x => (x.Phone == request.phone || x.Email == request.emial) && x.Id != defaultUser.Id);
            //if (isExist)
            //    throw ExceptionHelper.AppFriendly("电话或邮箱已存在");

            //更新企业信息
            info.Update(request.enterpriseName, info.EnterpriseTypeId, info.Pid, request.remark, request.areaRelationId);
            return await context.SaveChangesAsync() > 0;
        }
    }

    /// <summary>
    /// 企业注册审核
    /// </summary>
    /// <param name="context"></param>
    /// <param name="emailServiceProvider"></param>
    public class UpdateRegistrationAuditCommandHandler(CoffeeMachinePlatformDbContext context, IEmailServiceProvider emailServiceProvider) : ICommandHandler<UpdateRegistrationAuditCommand, bool>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(UpdateRegistrationAuditCommand request, CancellationToken cancellationToken)
        {
            var info = await context.EnterpriseInfo.FirstOrDefaultAsync(w => w.Id == request.id);
            if (info == null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            if (info.RegistrationProgress == RegistrationProgress.Failed || info.RegistrationProgress == RegistrationProgress.Passed)
                throw ExceptionHelper.AppFriendly("当前状态不能操作审核");
            if (request.registrationProgress != RegistrationProgress.Passed && request.registrationProgress != RegistrationProgress.Failed)
                throw ExceptionHelper.AppFriendly("更新状态只能是审核通过、审核失败");

            info.UpdateRegistrationProgress(request.registrationProgress);
            info.UpdateRemark(request.registrationProgress == RegistrationProgress.Passed ? string.Empty : request.remark);
            var res = context.EnterpriseInfo.Update(info);

            var userInfo = await context.ApplicationUser.FirstOrDefaultAsync(w => w.EnterpriseId == info.Id && w.IsDefault);
            if (userInfo != null)
            {
                var mailMessage = request.registrationProgress == RegistrationProgress.Passed
                    ? L.Text[nameof(ErrorCodeEnum.D0102)]
                    : L.Text[nameof(ErrorCodeEnum.D0103)];

                if (request.registrationProgress == RegistrationProgress.Passed)
                {
                    // 获取所有菜单
                    var menus = await context.ApplicationMenu.AsNoTracking().ToListAsync();
                    // 获取所有菜单Ids
                    var allMenusIds = menus.Where(w => w.SysMenuType == SysMenuTypeEnum.Merchant).Select(s => s.Id).ToList();
                    // 企业绑定菜单Ids
                    info.UpdateMenuIds(allMenusIds);

                    // 获取所有H5菜单Ids
                    var allH5MenusIds = menus.Where(w => w.SysMenuType == SysMenuTypeEnum.H5).Select(s => s.Id).ToList();
                    // 企业绑定菜单Ids
                    info.UpdateH5MenuIds(allH5MenusIds);
                }

                // 发送邮件
                var mailRes = await emailServiceProvider.SendEmailSingleAsync(new EmailObject() { ToEmail = userInfo.Email, MessageBody = mailMessage, Subject = L.Text[nameof(ErrorCodeEnum.D0104)] });
            }
            return res != null;
        }
    }
}