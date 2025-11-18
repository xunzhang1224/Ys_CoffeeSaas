using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageCollectionCommands
{
    /// <summary>
    /// 修改饮品集合名称
    /// </summary>
    /// <param name="id">饮品集合id</param>
    /// <param name="name">饮品集合名字</param>
    public record UpdateP_BeverageCollectionNameCommand(long id, string name) : ICommand<bool>;
}
