using System.Text.Json.Nodes;

namespace YS.CoffeeMachine.Domain.BasicDtos
{
    /// <summary>
    /// 饮品信息数据传输对象
    /// </summary>
    public class OrderBeverageInfoDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 分类Id集合
        /// </summary>
        public List<long>? CategoryIds { get; set; }

        /// <summary>
        /// 分类Id集合
        /// </summary>
        public List<string>? CategoryName { get; set; }

        /// <summary>
        /// 原价
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 折扣价
        /// </summary>
        public decimal? DiscountedPrice { get; set; }

        /// <summary>
        /// 配方列表
        /// </summary>
        public List<OrderFormulaInfoDto> FormulaInfos { get; set; } = [];

        /// <summary>
        /// 配方列表文本
        /// </summary>
        public string FormulasText { get { return string.Join(",", FormulaInfos.Select(s => s.FormulaType.GetDescriptionOrValue()).ToList()); } }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 物料拓展数据
        /// </summary>
        public JsonObject Specs { get; set; }

        /// <summary>
        /// 配方参数
        /// </summary>
        public string SpecsString { get; set; }

        /// <summary>
        /// 是否显示编码
        /// </summary>
        public bool CodeIsShow { get; set; }
    }
}