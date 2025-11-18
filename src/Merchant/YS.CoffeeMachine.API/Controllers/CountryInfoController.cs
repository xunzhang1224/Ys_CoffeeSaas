using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YS.CoffeeMachine.API.Extensions;
using YS.CoffeeMachine.Application.Dtos.CountryInfoDots;
using YS.CoffeeMachine.Application.Queries.ICountryAndRegionQueries;

namespace YS.CoffeeMachine.API.Controllers
{
    /// <summary>
    /// 国家信息查询
    /// </summary>
    /// <param name="queries"></param>
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(ApiManages.Merchant_v1))]
    public class CountryInfoController(ICountryInfoQueries queries) : Controller
    {
        /// <summary>
        /// 根据Id获取国家信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpPost("GetCountryInfoById")]
        public Task<CountryInfoDto> GetCountryInfoById(long id)
        {
            return queries.CountryInfoDtoAsync(id);
        }
        /// <summary>
        /// 获取国家列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("GetAllCountryInfoList")]
        public Task<CountryInfoListDto> GetAllCountryInfoList()
        {
            return queries.GetCountryInfoListDtosAsync();
        }
        /// <summary>
        /// 根据Id获取地区信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("GetCountryRegionById")]
        public Task<CountryRegionDto> GetCountryRegionById(long id)
        {
            return queries.GetCountryRegionDtoAsync(id);
        }
        /// <summary>
        /// 根据国家Id获取地区列表(省级地区)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("GetCountryRegionListDtosByCountryId")]
        public Task<CountryRegionListDto> GetCountryRegionListDtosByCountryId(long id)
        {
            return queries.GetCountryRegionListDtosByCountryIdAsync(id);
        }
        /// <summary>
        /// 根据上级地区Id获取地区列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("GetCountryRegionListDtosByParentId")]
        public Task<CountryRegionListDto> GetCountryRegionListDtosByParentId(long id)
        {
            return queries.GetCountryRegionListDtosByParentIdAsync(id);
        }
    }
}
