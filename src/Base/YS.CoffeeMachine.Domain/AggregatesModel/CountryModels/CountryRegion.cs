namespace YS.CoffeeMachine.Domain.AggregatesModel.CountryModels
{
    /// <summary>
    /// 地区
    /// </summary>
    public class CountryRegion
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; private set; }

        /// <summary>
        /// 地区名称
        /// </summary>
        public string RegionName { get; private set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        public long? ParentID { get; private set; }

        /// <summary>
        /// 标准地区编码
        /// </summary>
        public string? Code { get; private set; } = null;

        /// <summary>
        /// 父级编码
        /// </summary>
        public string? ParentCode { get; private set; } = null;

        /// <summary>
        /// 类型（0:国家，1:省份，2:城市，3:区县）
        /// </summary>
        public int? Type { get; private set; } = null;

        /// <summary>
        /// 是否有子级
        /// </summary>
        public bool HasChildren { get; private set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; private set; }

        /// <summary>
        /// 父级地区
        /// </summary>
        public CountryRegion ParentRegion { get; private set; }

        /// <summary>
        /// 子级地区
        /// </summary>
        public List<CountryRegion> Regions { get; private set; }

        /// <summary>
        /// 国家Id
        /// </summary>
        public long CountryID { get; private set; }

        /// <summary>
        /// 国家信息
        /// </summary>
        public CountryInfo CountryInfo { get; private set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public IsEnabledEnum IsEnabled { get; private set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; private set; }

        /// <summary>
        /// CountryRegion
        /// </summary>
        protected CountryRegion() { }

        /// <summary>
        /// CountryRegion
        /// </summary>
        /// <param name="regionName"></param>
        /// <param name="parentId"></param>
        /// <param name="countryId"></param>
        /// <param name="isEnabled"></param>
        public CountryRegion(string regionName, long? parentId, long countryId, IsEnabledEnum isEnabled)
        {
            RegionName = regionName;
            ParentID = parentId;
            CountryID = countryId;
            IsEnabled = isEnabled;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="regionName"></param>
        /// <param name="parentId"></param>
        /// <param name="isEnabled"></param>
        public void Update(string regionName, long parentId, IsEnabledEnum isEnabled)
        {
            RegionName = regionName;
            ParentID = parentId;
            IsEnabled = isEnabled;
        }

        /// <summary>
        /// 配置编码信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="parentCode"></param>
        /// <param name="type"></param>
        public void SetCodeInfo(string code, string parentCode, int type)
        {
            Code = code;
            ParentCode = parentCode;
            Type = type;
        }
    }
}
