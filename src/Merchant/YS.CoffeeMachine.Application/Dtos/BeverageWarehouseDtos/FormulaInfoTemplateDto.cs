using System.Text.Json.Nodes;
using YS.CoffeeMachine.Application.Tools;
using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;

namespace YS.CoffeeMachine.Application.Dtos.BeverageWarehouseDtos
{
    /// <summary>
    /// 配方信息
    /// </summary>
    public class FormulaInfoTemplateDto
    {
        /// <summary>
        /// 饮品Id
        /// </summary>
        public long BeverageInfoTemplateId { get; private set; }
        /// <summary>
        /// 料盒
        /// </summary>
        public long? MaterialBoxId { get; set; }
        /// <summary>
        /// 料盒位置
        /// </summary>
        //public int MaterialSort { get; set; }
        /// <summary>
        /// 料盒名称
        /// </summary>
        public string MaterialBoxName { get; set; }
        /// <summary>
        /// 配方类型
        /// </summary>
        public FormulaTypeEnum FormulaType { get; set; }
        /// <summary>
        /// 配方类型描述
        /// </summary>
        public string FormulaTypeText { get { return FormulaType.GetDescriptionOrValue(); } }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 物料拓展数据
        /// </summary>
        public JsonObject Specs { get; set; }

        /// <summary>
        /// 物料拓展数据
        /// </summary>
        public string SpecsString { get; set; }
    }
}
