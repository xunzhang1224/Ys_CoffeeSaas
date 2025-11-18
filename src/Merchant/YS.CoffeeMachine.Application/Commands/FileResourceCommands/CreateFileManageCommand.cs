using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.Files;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.FileResourceCommands
{
    public record class CreateFileManageCommand(FileManageInput file) : ICommand;
}
