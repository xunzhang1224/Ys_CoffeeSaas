using YS.CoffeeMachine.Application.Dtos.Cap;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageCollectionCommands
{
    /// <summary>
    /// 应用默认饮品集合到指定设备
    /// </summary>
    /// <param name="id"></param>
    public record AppliedDefaultBeverageCollectionCommand(long id, long deviceId) : ICommand<CommandDownSends>;
}