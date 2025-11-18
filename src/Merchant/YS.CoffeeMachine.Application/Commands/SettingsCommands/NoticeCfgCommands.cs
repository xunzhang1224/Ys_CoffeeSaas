using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.SettringDtos;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.SettingsCommands
{
    public record SetNoticeCfgCommand(NoticeCfgInput NoticeCfgs) : ICommand<bool>;
}
