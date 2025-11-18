using YS.CoffeeMachine.Application.Dtos.CountryInfoDots;

namespace YS.CoffeeMachine.Application.Queries.ICountryAndRegionQueries
{
    /// <summary>
    /// 国家地区查询接口
    /// </summary>
    public interface ICountryInfoQueries
    {
        /// <summary>
        /// 根据Id获取国家信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CountryInfoDto> CountryInfoDtoAsync(long id);

        /// <summary>
        /// 获取国家列表
        /// </summary>
        /// <returns></returns>
        Task<CountryInfoListDto> GetCountryInfoListDtosAsync();

        /// <summary>
        /// 根据Id获取地区信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CountryRegionDto> GetCountryRegionDtoAsync(long id);

        /// <summary>
        /// 根据国家Id获取地区列表(省级地区)
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        Task<CountryRegionListDto> GetCountryRegionListDtosByCountryIdAsync(long countryId);

        /// <summary>
        /// 根据上级地区Id获取地区列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        Task<CountryRegionListDto> GetCountryRegionListDtosByParentIdAsync(long parentId);
    }
}