using YS.CoffeeMachine.Domain.AggregatesModel.UserInfo;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.ApplicationCommands.EnterpriseCommands
{
    public record UpdateEnterPriseMenuIdsCommand(long id, SysMenuTypeEnum SysMenuType, List<long> ids, List<long>? halfMenuIds = null, List<long>? childHalfMenuIds = null) : ICommand<bool>;
}