using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YSCore.Base.Events;
namespace YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoCommands
{
    /// <summary>
    /// 更新饮品信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="beverageIcon"></param>
    /// <param name="temperature"></param>
    /// <param name="remarks"></param>
    /// <param name="formulaInfoDtos"></param>
    /// <param name="productionForecast"></param>
    /// <param name="forecastQuantity"></param>
    /// <param name="displayStatus"></param>
    public record UpdateBeverageInfoCommand(long id, long deviceModelId, string name, string beverageIcon, TemperatureEnum temperature, string remarks, List<FormulaInfoDto> formulaInfos,
        string productionForecast, double forecastQuantity, bool displayStatus) : ICommand<bool>;
}
