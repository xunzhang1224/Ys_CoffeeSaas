namespace YS.CoffeeMachine.Domain.AggregatesModel.Currencys
{
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 表示一个聚合根实体，用于管理货币相关的信息。
    /// 该类继承自 BaseEntity，并实现了 IAggregateRoot 接口，表明其在领域模型中作为聚合根的角色。
    /// 主要用于存储和管理与货币相关的区域性信息，如文化名称、货币符号、货币代码、区域名称和国家名称等。
    /// </summary>
    public class CurrencyInfo : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 获取或设置货币对应的文化名称（例如 "en-US" 或 "zh-CN"）。
        /// </summary>
        public string Culture { get; private set; }

        /// <summary>
        /// 获取或设置货币的符号（例如 "$" 或 "¥"）。
        /// </summary>
        public string CurrencySymbol { get; private set; }

        /// <summary>
        /// 获取或设置货币的ISO代码（例如 "USD" 或 "CNY"）。
        /// </summary>
        public string CurrencyCode { get; private set; }

        /// <summary>
        /// 获取或设置货币所属的区域名称（例如 "North America" 或 "Asia"）。
        /// </summary>
        public string RegionName { get; private set; }

        /// <summary>
        /// 获取或设置货币所属的国家名称（例如 "United States" 或 "China"）。
        /// </summary>
        public string CountryName { get; private set; }

        /// <summary>
        /// 受保护的无参构造函数，供EF Core等ORM工具使用。
        /// </summary>
        protected CurrencyInfo() { }

        /// <summary>
        /// 使用指定参数初始化一个新的 CurrencyInfo 实例。
        /// </summary>
        /// <param name="culture">货币对应的文化名称。</param>
        /// <param name="currencySymbol">货币的符号。</param>
        /// <param name="currencyCode">货币的ISO代码。</param>
        /// <param name="regionName">货币所属的区域名称。</param>
        /// <param name="countryName">货币所属的国家名称。</param>
        public CurrencyInfo(string culture, string currencySymbol, string currencyCode, string regionName, string countryName)
        {
            Culture = culture;
            CurrencySymbol = currencySymbol;
            CurrencyCode = currencyCode;
            RegionName = regionName;
            CountryName = countryName;
        }
    }
}