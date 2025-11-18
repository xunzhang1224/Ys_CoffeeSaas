using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YS.CoffeeMachine.Application.Dtos.Files;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YSCore.Base.Events;
namespace YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoCommands
{
    public record UpdateBeverageInfoCommand(long id, string name, string beverageIcon, string code, TemperatureEnum temperature, string remarks, List<FormulaInfoDto> formulaInfoDtos,
        string productionForecast, double forecastQuantity, bool displayStatus, string sellStradgy, FileManageInput? file, decimal? price, decimal? discountedPrice, List<long>? productCategoryIds = null) : ICommand<bool>;
}
