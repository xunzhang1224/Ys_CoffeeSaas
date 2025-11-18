using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.ApplicationCommands.ServiceAuthorizationRecordCommands
{
    public record UpdateServiceAuthorizationRecordCommand(long id, ServiceAuthorizationStateEnum state) : ICommand<bool>;
}
