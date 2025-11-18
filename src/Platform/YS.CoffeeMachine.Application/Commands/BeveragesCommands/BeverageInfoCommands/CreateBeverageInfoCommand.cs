using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoCommands
{
    /// <summary>
    /// 创建饮品
    /// </summary>
    /// <param name="deviceModelId">设备型号id</param>
    /// <param name="name">饮品名称</param>
    /// <param name="beverageIcon">饮品图片</param>
    /// <param name="code">饮品编码</param>
    /// <param name="temperature">温度</param>
    /// <param name="remarks">备注</param>
    /// <param name="formulaInfos">配料集合</param>
    /// <param name="productionForecast"></param>
    /// <param name="forecastQuantity">预测总量</param>
    /// <param name="displayStatus">是否启用</param>
    public record CreateBeverageInfoCommand(long deviceModelId, string name, string beverageIcon, string code, TemperatureEnum temperature, string remarks, List<FormulaInfoDto> formulaInfos,
        string productionForecast, double forecastQuantity, bool displayStatus, string languageKey) : ICommand<bool>;
}
