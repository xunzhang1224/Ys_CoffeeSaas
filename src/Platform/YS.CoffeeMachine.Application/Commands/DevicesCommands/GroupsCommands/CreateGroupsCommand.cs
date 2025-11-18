using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.DevicesCommands.GroupsCommands
{
    public record CreateGroupsCommand(long enterpriseInfoId, string name, long? pid, List<long>? userIds, List<long>? deviceIds, string Remark) : ICommand<bool>;
}
