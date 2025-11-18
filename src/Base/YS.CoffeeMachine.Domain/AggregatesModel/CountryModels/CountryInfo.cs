namespace YS.CoffeeMachine.Domain.AggregatesModel.CountryModels
{
    using YSCore.Base.DatabaseAccessor;

    /// <summary>
    /// 国家信息聚合根
    /// </summary>
    public class CountryInfo : IAggregateRoot
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 国家名称
        /// </summary>
        public string CountryName { get; private set; }

        /// <summary>
        /// 国家代码，例如“CN”
        /// </summary>
        public string CountryCode { get; private set; }

        /// <summary>
        /// 所在大洲
        /// </summary>
        public string Continent { get; private set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public IsEnabledEnum IsEnabled { get; private set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; private set; }

        /// <summary>
        /// 国家区域列表
        /// </summary>
        public List<CountryRegion> Regions { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected CountryInfo() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="countryName">国家名称</param>
        /// <param name="countryCode">国家代码</param>
        /// <param name="continent">所在大陆</param>
        /// <param name="isEnabled">是否启用</param>
        public CountryInfo(string countryName, string countryCode, string continent, IsEnabledEnum isEnabled)
        {
            CountryName = countryName;
            CountryCode = countryCode;
            Continent = continent;
            IsEnabled = isEnabled;
        }

        /// <summary>
        /// 更新国家信息
        /// </summary>
        /// <param name="countryName">国家名称</param>
        /// <param name="countryCode">国家代码</param>
        /// <param name="continent">所在大陆</param>
        /// <param name="isEnabled">是否启用</param>
        public void Update(string countryName, string countryCode, string continent, IsEnabledEnum isEnabled)
        {
            CountryName = countryName;
            CountryCode = countryCode;
            Continent = continent;
            IsEnabled = isEnabled;
        }
    }
}
