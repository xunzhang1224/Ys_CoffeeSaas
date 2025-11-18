using YS.CoffeeMachine.Application.Dtos.BeverageWarehouseDtos;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoTemplateCommands
{
    public record CreateBeverageInfoTemplateCommand(long enterpriseInfoId, long deviceModelId, string name, string beverageIcon, string code, bool codeIsShow, TemperatureEnum temperature, string remarks, List<FormulaInfoTemplateDto> formulaInfoTemplateDto, string productionForecast, double forecastQuantity, bool displayStatus, string sellStradgy = null, List<long>? productCategoryIds = null) : ICommand<bool>;
}
