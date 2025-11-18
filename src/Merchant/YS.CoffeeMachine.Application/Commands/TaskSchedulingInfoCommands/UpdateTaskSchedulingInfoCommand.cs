using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.TaskSchedulingInfoCommands
{
    public record UpdateTaskSchedulingInfoCommand(long id, string name, string description, string cron, bool isEnabled) : ICommand<bool>;
};
