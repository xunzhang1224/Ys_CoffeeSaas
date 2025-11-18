using MediatR;
using YS.CoffeeMachine.Application.Commands.ApplicationCommands.EnterpriseCommands;
using YS.CoffeeMachine.Domain.Events.ApplicationDomainEvents;
using YSCore.Base.DomainEvent;

namespace YS.CoffeeMachine.Platform.API.Application.DomainEventHandlers.ApplicationDomainEventHandlers
{
    /// <summary>
    /// 创建应用角色领域事件处理
    /// </summary>
    /// <param name="mediator"></param>
    public class CreateApplicationRoleDomainEventHandler(IMediator mediator) : IDomainEventHandler<CreateApplicationRoleDomainEvent>
    {
        /// <summary>
        /// 创建应用角色领域事件处理
        /// </summary>
        public async Task Handle(CreateApplicationRoleDomainEvent notification, CancellationToken cancellationToken)
        {
            var info = notification.EnterpriseInfo;
            info.UpdateEnterpriseRoles(new List<long>() { notification.roleId });
            var command = new UpdateEnterpriseInfoCommand(info.Id, info.Name, info.EnterpriseTypeId, info.Pid, info.Users.Select(s => s.Id).ToList(), info.Roles.Select(s => s.Id).ToList(), info.Remark);
            var res = await mediator.Send(command);
        }
    }
}
