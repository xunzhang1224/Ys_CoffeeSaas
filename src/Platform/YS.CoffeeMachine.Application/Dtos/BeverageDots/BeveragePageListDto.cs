namespace YS.CoffeeMachine.Application.Dtos.BeverageDots
{
    /// <summary>
    /// 饮品分页dto
    /// </summary>
    public class BeveragePageListDto
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 饮品图标
        /// </summary>
        public string BeverageIcon { get; set; }
        /// <summary>
        /// 编码（商品SKU），同设备内唯一
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 出品预测
        /// </summary>
        public string ProductionForecast { get; set; }
        /// <summary>
        /// 出杯量预测(ml)
        /// </summary>
        public double? ForecastQuantity { get; set; }
        /// <summary>
        /// 饮品元素
        /// </summary>
        public string FormulasText { get; set; }
        /// <summary>
        /// 配方版本
        /// </summary>
        public string VersionText { get; set; }
    }
}
