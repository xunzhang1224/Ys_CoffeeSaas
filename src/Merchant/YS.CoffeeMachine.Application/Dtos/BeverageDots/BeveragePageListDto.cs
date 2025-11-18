namespace YS.CoffeeMachine.Application.Dtos.BeverageDots
{
    /// <summary>
    /// 饮品分页列表
    /// </summary>
    public class BeveragePageListDto
    {
        /// <summary>
        /// 饮品ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 分类Id集合
        /// </summary>
        public List<long>? CategoryIds { get; set; }

        /// <summary>
        /// 分类名称集合
        /// </summary>
        public string CategoryNames { get; set; }

        /// <summary>
        /// 原价
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// 折扣价
        /// </summary>
        public decimal? DiscountedPrice { get; set; }

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
        /// 是否显示SKU编码
        /// </summary>
        public bool CodeIsShow { get; set; }
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

        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort { get; set; }

        /// <summary>
        /// 售卖策略
        /// </summary>
        public string SellStradgy { get; set; }

        /// <summary>
        /// 显示状态
        /// </summary>
        public bool DisplayStatus { get; set; }

        /// <summary>
        /// 自动编号
        /// </summary>
        public long? AutoCode { get; set; }
    }

    /// <summary>
    /// 饮品信息
    /// </summary>
    public class BeverageInfoSubDto
    {

    }
}
