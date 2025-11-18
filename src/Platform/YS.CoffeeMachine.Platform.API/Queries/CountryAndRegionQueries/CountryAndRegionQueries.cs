using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Dtos.CountryInfoDots;
using YS.CoffeeMachine.Application.Queries.ICountryAndRegionQueries;
using YS.CoffeeMachine.Domain.AggregatesModel.CountryModels;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;

namespace YS.CoffeeMachine.Platform.API.Queries.CountryAndRegionQueries
{
    /// <summary>
    /// 国家信息查询
    /// </summary>
    /// <param name="context"></param>
    public class CountryInfoQueries(CoffeeMachineDbContext context) : ICountryInfoQueries
    {
        /// <summary>
        /// 根据Id获取国家信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task<CountryInfoDto> CountryInfoDtoAsync(long id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            var info = await context.CountryInfo.FirstOrDefaultAsync(w => w.Id == id && w.IsEnabled == IsEnabledEnum.Enabled) ?? throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            var countryInfoDto = new CountryInfoDto(info.Id, info.CountryName, info.CountryCode, info.Continent);
            return countryInfoDto;
        }
        /// <summary>
        /// 获取国家列表
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CountryInfoListDto> GetCountryInfoListDtosAsync()
        {
            var info = await context.CountryInfo.AsNoTracking().Where(w => w.IsEnabled == IsEnabledEnum.Enabled).ToListAsync();
            if (info == null || info.Count == 0)
                return new CountryInfoListDto();
            var countryInfoList = new CountryInfoListDto();
            countryInfoList.CountryInfoList = new List<CountryInfoDto>();
            foreach (var country in info)
                countryInfoList.CountryInfoList.Add(new CountryInfoDto(country.Id, country.CountryName, country.CountryCode, country.Continent));
            return countryInfoList;
        }
        /// <summary>
        /// 根据Id获取地区信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CountryRegionDto> GetCountryRegionDtoAsync(long id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            var info = await context.CountryRegion.FirstOrDefaultAsync(w => w.Id == id && w.IsEnabled == IsEnabledEnum.Enabled) ?? throw new ArgumentException(L.Text[nameof(ErrorCodeEnum.D0014)]);
            return new CountryRegionDto(info.Id, info.RegionName, info.ParentID, info.HasChildren, info.CountryID);
        }
        /// <summary>
        /// 根据国家Id获取地区列表(省级地区)
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CountryRegionListDto> GetCountryRegionListDtosByCountryIdAsync(long countryId)
        {
            if (countryId <= 0)
                throw new ArgumentOutOfRangeException(nameof(countryId));
            var info = await context.CountryInfo
                            .AsNoTracking()
                            .Where(w => w.Id == countryId && w.IsEnabled == IsEnabledEnum.Enabled)
                            .Include(i => i.Regions)
                            .Select(country => new
                            {
                                country.Id,
                                country.CountryName,
                                Regions = country.Regions
                                    .Where(region => region.ParentID == null && region.IsEnabled == IsEnabledEnum.Enabled).OrderBy(s => s.Sort)
                                    .ToList()
                            })
                            .FirstOrDefaultAsync() ?? throw new ArgumentException(L.Text[nameof(ErrorCodeEnum.D0014)]);
            var countryRegionListDto = new CountryRegionListDto();
            foreach (var region in info.Regions)
                countryRegionListDto.CountryRegionList.Add(new CountryRegionDto(region.Id, region.RegionName, region.ParentID, region.HasChildren, region.CountryID));
            return countryRegionListDto;
        }
        /// <summary>
        /// 根据上级地区Id获取地区列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CountryRegionListDto> GetCountryRegionListDtosByParentIdAsync(long parentId)
        {
            if (parentId <= 0)
                throw new ArgumentOutOfRangeException(nameof(parentId));
            var info = await context.CountryRegion.Where(w => w.ParentID == parentId).ToListAsync();
            var countryRegionListDto = new CountryRegionListDto();
            foreach (var region in info)
                countryRegionListDto.CountryRegionList.Add(new CountryRegionDto(region.Id, region.RegionName, region.ParentID, region.HasChildren, region.CountryID));
            return countryRegionListDto;
        }
    }
}
