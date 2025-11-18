namespace YS.CoffeeMachine.Domain.AggregatesModel.BeverageWarehouse
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json;
    using System.Text.Json.Nodes;
    using YS.CoffeeMachine.Domain.AggregatesModel.Beverages;
    using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 配方物料模板
    /// </summary>
    public class FormulaInfoTemplate : BaseEntity
    {
        /// <summary>
        /// 饮品Id
        /// </summary>
        public long BeverageInfoTemplateId { get; private set; }

        /// <summary>
        /// 料盒
        /// </summary>
        public long? MaterialBoxId { get; private set; }

        /// <summary>
        /// 料盒名称
        /// </summary>
        public string MaterialBoxName { get; private set; }

        /// <summary>
        /// 料盒
        /// </summary>
        public MaterialBox MaterialBox { get; private set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; private set; }
        private string _extendedData;

        /// <summary>
        /// 配方类型
        /// </summary>
        public FormulaTypeEnum FormulaType { get; private set; }

        /// <summary>
        /// 忽略
        /// </summary>
        [NotMapped]
        public JsonObject Specs
        {
            get
            {
                // 将 JSON 字符串反序列化为 JObject
                return JsonSerializer.Deserialize<JsonObject>(string.IsNullOrWhiteSpace(_extendedData) ? null : _extendedData);
            }
            set
            {
                // 将 JObject 序列化为字符串
                _extendedData = value.ToString();
            }
        }

        /// <summary>
        /// 配方物料拓展数据
        /// </summary>
        public string SpecsString
        {
            get => _extendedData;
            set => _extendedData = value;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected FormulaInfoTemplate() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="beverageInfoTemplateId"></param>
        /// <param name="materialBoxId"></param>
        /// <param name="materialBoxName"></param>
        /// <param name="sort"></param>
        /// <param name="formulaType"></param>
        /// <param name="specs"></param>
        public FormulaInfoTemplate(long beverageInfoTemplateId, long? materialBoxId, string materialBoxName, int sort, FormulaTypeEnum formulaType, JsonObject specs)
        {
            BeverageInfoTemplateId = beverageInfoTemplateId;
            MaterialBoxId = materialBoxId;
            MaterialBoxName = materialBoxName;
            Sort = sort;
            FormulaType = formulaType;
            Specs = specs;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="beverageInfoTemplateId"></param>
        /// <param name="materialBoxId"></param>
        /// <param name="materialBoxName"></param>
        /// <param name="sort"></param>
        /// <param name="formulaType"></param>
        /// <param name="specs"></param>
        public void Update(long beverageInfoTemplateId, long? materialBoxId, string materialBoxName, int sort, FormulaTypeEnum formulaType, JsonObject specs)
        {
            BeverageInfoTemplateId = beverageInfoTemplateId;
            MaterialBoxId = materialBoxId;
            MaterialBoxName = materialBoxName;
            Sort = sort;
            FormulaType = formulaType;
            Specs = specs;
        }
    }
}
