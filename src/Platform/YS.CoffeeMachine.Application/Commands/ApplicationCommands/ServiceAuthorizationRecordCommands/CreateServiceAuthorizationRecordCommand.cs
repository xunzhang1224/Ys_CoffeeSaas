using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.ApplicationCommands.ServiceAuthorizationRecordCommands
{
    public record CreateServiceAuthorizationRecordCommand(string name, long founderId, List<long> deviceIds, string serviceUserAccount, long serviceUserId, DateTime? authorizationEndTime) : ICommand<bool>;
}
