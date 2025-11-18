using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageCollectionCommands
{
    /// <summary>
    /// 删除饮品集合
    /// </summary>
    /// <param name="ids"></param>
    public record DeleteP_BeverageCollectionCommand(List<long> ids) : ICommand<bool>;
}
