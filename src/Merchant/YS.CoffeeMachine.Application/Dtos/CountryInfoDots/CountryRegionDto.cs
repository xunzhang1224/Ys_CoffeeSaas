namespace YS.CoffeeMachine.Application.Dtos.CountryInfoDots
{
    /// <summary>
    /// 国家地区
    /// </summary>
    public class CountryRegionDto
    {
        /// <summary>
        /// 地区Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 地区名称
        /// </summary>
        public string RegionName { get; set; }
        /// <summary>
        /// 父级Id
        /// </summary>
        public long? ParentID { get; set; }
        /// <summary>
        /// 是否有子级
        /// </summary>
        public bool HasChildren { get; set; }
        /// <summary>
        /// 国家Id
        /// </summary>
        public long CountryID { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CountryRegionDto(long id, string regionName, long? parentId, bool hasChildren, long countryId)
        {
            Id = id;
            RegionName = regionName;
            ParentID = parentId;
            HasChildren = hasChildren;
            CountryID = countryId;
        }
    }

    /// <summary>
    /// 国家地区列表
    /// </summary>
    public class CountryRegionListDto
    {
        /// <summary>
        /// 国家地区列表
        /// </summary>
        public List<CountryRegionDto> CountryRegionList { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CountryRegionListDto() { CountryRegionList = new List<CountryRegionDto>(); }
    }
}
