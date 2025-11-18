namespace YS.CoffeeMachine.Domain.AggregatesModel.Beverages
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json;
    using System.Text.Json.Nodes;
    using YS.CoffeeMachine.Domain.AggregatesModel.Settings;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 配方物料
    /// </summary>
    public class P_FormulaInfo : BaseEntity
    {
        /// <summary>
        /// 饮品Id
        /// </summary>
        public long BeverageInfoId { get; private set; }

        /// <summary>
        /// 料盒
        /// </summary>
        public long? MaterialBoxId { get; private set; }

        /// <summary>
        /// 料盒名称
        /// </summary>
        public string MaterialBoxName { get; private set; }

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
        protected P_FormulaInfo() { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="beverageInfoId"></param>
        /// <param name="materialBoxId"></param>
        /// <param name="materialBoxName"></param>
        /// <param name="sort"></param>
        /// <param name="formulaType"></param>
        /// <param name="specs"></param>
        public P_FormulaInfo(long beverageInfoId, long? materialBoxId, string materialBoxName, int sort, FormulaTypeEnum formulaType, JsonObject specs)
        {
            BeverageInfoId = beverageInfoId;
            MaterialBoxId = materialBoxId;
            MaterialBoxName = materialBoxName;
            Sort = sort;
            FormulaType = formulaType;
            Specs = specs;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="beverageInfoId"></param>
        /// <param name="materialBoxId"></param>
        /// <param name="materialBoxName"></param>
        /// <param name="sort"></param>
        /// <param name="formulaType"></param>
        /// <param name="specs"></param>
        public void Update(long beverageInfoId, long? materialBoxId, string materialBoxName, int sort, FormulaTypeEnum formulaType, JsonObject specs)
        {
            BeverageInfoId = beverageInfoId;
            MaterialBoxId = materialBoxId;
            MaterialBoxName = materialBoxName;
            Sort = sort;
            FormulaType = formulaType;
            Specs = specs;
        }
    }
}
