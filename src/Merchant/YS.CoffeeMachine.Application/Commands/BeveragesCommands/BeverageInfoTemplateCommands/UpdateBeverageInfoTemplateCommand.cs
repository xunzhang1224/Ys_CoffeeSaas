using YS.CoffeeMachine.Application.Dtos.BeverageWarehouseDtos;
using YS.CoffeeMachine.Application.Dtos.Files;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoTemplateCommands
{
    public record UpdateBeverageInfoTemplateCommand(long id, string name, string beverageIcon, string code, TemperatureEnum temperature, string remarks, List<FormulaInfoTemplateDto> formulaInfoTemplateDto,
        string productionForecast, double forecastQuantity, bool displayStatus, string sellStradgy = null, FileManageInput? file = null, List<long>? productCategoryIds = null) : ICommand<bool>;
}
