namespace YS.CoffeeMachine.Domain.AggregatesModel.Strategy
{
    using YS.CoffeeMachine.Domain.Shared.Enum;
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 地区关联（策略）
    /// </summary>
    public class AreaRelation : BaseEntity
    {
        /// <summary>
        /// 地区（字典）
        /// </summary>
        public string Area { get; private set; }

        /// <summary>
        /// 国家（字典）
        /// </summary>
        public string Country { get; private set; }

        /// <summary>
        /// 区号
        /// </summary>
        public string AreaCode { get; private set; }

        /// <summary>
        /// 语言（字典）
        /// </summary>
        public string Language { get; private set; }

        /// <summary>
        /// 币种（字典）
        /// </summary>
        public long CurrencyId { get; private set; }

        /// <summary>
        /// 时区
        /// </summary>
        public string TimeZone { get; private set; }

        /// <summary>
        /// 服务条款Url
        /// </summary>
        public string TermServiceUrl { get; private set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public EnabledEnum Enabled { get; private set; }

        /// <summary>
        /// 地区关联
        /// </summary>
        protected AreaRelation() { }

        /// <summary>
        /// 创建地区关联
        /// </summary>
        public AreaRelation(string area, string country, string areaCode, string language, long currencyId, string timeZone, string termServiceUrl, EnabledEnum enabled)
        {
            Area = area;
            Country = country;
            AreaCode = areaCode;
            Language = language;
            CurrencyId = currencyId;
            TimeZone = timeZone;
            TermServiceUrl = termServiceUrl;
            Enabled = enabled;
        }

        /// <summary>
        /// 更新地区关联
        /// </summary>
        /// </summary>
        /// <param name="area"></param>
        /// <param name="country"></param>
        /// <param name="areaCode"></param>
        /// <param name="language"></param>
        /// <param name="currencyId"></param>
        /// <param name="timeZone"></param>
        /// <param name="termServiceUrl"></param>
        /// <param name="enabled"></param>
        public void UpdateAreaRelation(string area, string country, string areaCode, string language, long currencyId, string timeZone, string termServiceUrl, EnabledEnum enabled)
        {
            Area = area;
            Country = country;
            AreaCode = areaCode;
            Language = language;
            CurrencyId = currencyId;
            TimeZone = timeZone;
            TermServiceUrl = termServiceUrl;
            Enabled = enabled;
        }
    }
}
