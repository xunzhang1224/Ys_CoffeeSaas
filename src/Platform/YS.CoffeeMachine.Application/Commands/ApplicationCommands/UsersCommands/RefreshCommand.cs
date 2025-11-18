using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YS.CoffeeMachine.Application.Dtos.ApplicationInfoDtos;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.ApplicationCommands.UsersCommands
{
    public record RefreshCommand(string refreshToken) : ICommand<LoginResponseDto>;
}
