namespace YS.CoffeeMachine.Application.Dtos.CountryInfoDots
{
    /// <summary>
    /// 国家地区dto
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
        /// CountryRegionDto
        /// </summary>
        /// <param name="id"></param>
        /// <param name="regionName"></param>
        /// <param name="parentId"></param>
        /// <param name="hasChildren"></param>
        /// <param name="countryId"></param>
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
    /// CountryRegionListDto
    /// </summary>
    public class CountryRegionListDto
    {
        /// <summary>
        /// CountryRegionList
        /// </summary>
        public List<CountryRegionDto> CountryRegionList { get; set; }

        /// <summary>
        /// CountryRegionListDto
        /// </summary>
        public CountryRegionListDto() { CountryRegionList = new List<CountryRegionDto>(); }
    }
}
