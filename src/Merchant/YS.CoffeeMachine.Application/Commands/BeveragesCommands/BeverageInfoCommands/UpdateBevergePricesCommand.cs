using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoCommands
{
    public record UpdateBevergePricesCommand(List<PriceInfoDot> PriceInfos) : ICommand<bool>;
}
