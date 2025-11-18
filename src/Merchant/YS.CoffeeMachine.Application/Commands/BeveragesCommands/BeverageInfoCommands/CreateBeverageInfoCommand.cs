using YS.CoffeeMachine.Application.Dtos.BeverageDots;
using YS.CoffeeMachine.Application.Dtos.Files;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
using YSCore.Base.Events;

namespace YS.CoffeeMachine.Application.Commands.BeveragesCommands.BeverageInfoCommands
{
    public record CreateBeverageInfoCommand(long deviceInfoId, string name, string beverageIcon, string code, TemperatureEnum temperature, string remarks,
        List<FormulaInfoDto> formulaInfoDtos, string productionForecast, double forecastQuantity, bool displayStatus, FileManageInput? file, string? sellStradgy,
        decimal? price, decimal? discountedPrice, List<long>? productCategoryIds = null) : ICommand<string>;

    /// <summary>
    /// 创建商品信息
    /// </summary>
    public class CreateBeverageInfoCommandInput
    {
        /// <summary>
        /// 设备信息Id
        /// </summary>
        public long deviceInfoId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 商品图标
        /// </summary>
        public string beverageIcon { get; set; }

        /// <summary>
        /// 商品编码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// temperature
        /// </summary>
        public TemperatureEnum temperature { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        public string remarks { get; set; }

        /// <summary>
        /// 配方信息
        /// </summary>
        public List<FormulaInfoDto> formulaInfoDtos { get; set; }

        /// <summary>
        /// 生产预测
        /// </summary>
        public string productionForecast { get; set; }

        /// <summary>
        /// 预测数量
        /// </summary>
        public double forecastQuantity { get; set; }

        /// <summary>
        /// 显示状态
        /// </summary>
        public bool displayStatus { get; set; }

        /// <summary>
        /// 售Strategy
        /// </summary>
        public string? sellStradgy { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal? price { get; set; }

        /// <summary>
        /// 优惠价格
        /// </summary>
        public decimal? discountedPrice { get; set; }

    }
}
