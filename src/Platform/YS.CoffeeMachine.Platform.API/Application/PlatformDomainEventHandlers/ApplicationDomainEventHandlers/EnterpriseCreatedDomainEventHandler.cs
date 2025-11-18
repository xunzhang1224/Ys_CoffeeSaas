using MediatR;
using YS.CoffeeMachine.Application.Commands.ApplicationCommands.UsersCommands;
using YS.CoffeeMachine.Application.IServices;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YS.CoffeeMachine.Domain.PlatformEvents.ApplicationDomainEvents;
using YSCore.Base.DomainEvent;

namespace YS.CoffeeMachine.Platform.API.Application.PlatformDomainEventHandlers.ApplicationDomainEventHandlers
{
    /// <summary>
    /// 企业创建事件，添加超级管理员
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="passwordHasher"></param>
    public class EnterpriseCreatedDomainEventHandler(IMediator mediator, IPasswordHasher passwordHasher) : IDomainEventHandler<EnterpriseCreatedDomainEvent>
    {
        /// <summary>
        /// 企业创建事件，添加超级管理员
        /// </summary>
        public async Task Handle(EnterpriseCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var password = "123456";// passwordHasher.CreateRandomPassword();
            //发送邮件操作

            //创建超级管理员
            var command = new CreateApplicationUserCommand(notification.enterpriseId, notification.account, passwordHasher.HashPassword(password), notification.nickName, notification.areaCode, notification.phone, notification.emial,
                "默认账号", new List<long>(), SysMenuTypeEnum.Platform);
            var res = await mediator.Send(command);
        }
    }
}