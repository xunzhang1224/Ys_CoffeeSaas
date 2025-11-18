namespace YS.CoffeeMachine.Domain.AggregatesModel.Basics
{
    using YS.CoffeeMachine.Domain.Shared.Enum;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 货币
    /// </summary>
    public class Currency : BaseEntity
    {
        /// <summary>
        /// 货币代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 货币名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 货币符号
        /// </summary>
        public string CurrencySymbol { get; set; }

        /// <summary>
        /// 默认显示格式
        /// </summary>
        public CurrencyShowFormatEnum CurrencyShowFormat { get; set; }

        /// <summary>
        /// 金额精度
        /// </summary>
        public int Accuracy { get; set; }

        /// <summary>
        /// 舍入类型
        /// </summary>
        public RoundingTypeEnum RoundingType { get; set; }

        /// <summary>
        /// 启用状态
        /// </summary>
        public EnabledEnum Enabled { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected Currency() { }

        /// <summary>
        /// 新增
        /// </summary>
        public Currency(string code, string name, string currencySymbol, CurrencyShowFormatEnum currencyShowFormat, int accuracy, RoundingTypeEnum roundingType, EnabledEnum enabled)
        {
            Code = code;
            Name = name;
            CurrencySymbol = currencySymbol;
            CurrencyShowFormat = currencyShowFormat;
            Accuracy = accuracy;
            RoundingType = roundingType;
            Enabled = enabled;
        }

        /// <summary>
        /// 更新货币信息
        /// </summary>
        /// <param name="name">货币名称</param>
        /// <param name="currencySymbol">货币符号</param>
        /// <param name="currencyShowFormat">默认显示格式</param>
        /// <param name="enabled">启用状态</param>
        public void UpdateCurrency(string name, string currencySymbol, CurrencyShowFormatEnum currencyShowFormat, EnabledEnum enabled)
        {
            Name = name;
            CurrencySymbol = currencySymbol;
            CurrencyShowFormat = currencyShowFormat;
            Enabled = enabled;
        }
    }
}
