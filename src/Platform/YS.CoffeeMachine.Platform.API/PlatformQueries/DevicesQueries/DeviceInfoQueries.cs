using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YS.CoffeeMachine.Application.Dtos.DeviceDots;
using YS.CoffeeMachine.Application.Dtos.PagingDto;
using YS.CoffeeMachine.Application.Dtos.SettringDtos;
using YS.CoffeeMachine.Application.PlatformDto.DeviceDots;
using YS.CoffeeMachine.Application.PlatformQueries.IDevicesQueries;
using YS.CoffeeMachine.Application.Tools;
using YS.CoffeeMachine.Domain.AggregatesModel.Devices;
using YS.CoffeeMachine.Infrastructure;
using YS.CoffeeMachine.Localization;
using YS.CoffeeMachine.Platform.API.Queries;
using YS.CoffeeMachine.Platform.API.Queries.Pagings;
using YSCore.Base.Exceptions;
using YSCore.Base.Localization;
using YSCore.Base.Pagelist;
using YSCore.Provider.EntityFrameworkCore.Extensions;

namespace YS.CoffeeMachine.Platform.API.PlatformQueries.DevicesQueries
{
    /// <summary>
    /// 平台端设备列表查询
    /// </summary>
    public class DeviceInfoQueries(CoffeeMachinePlatformDbContext context, IMapper mapper) : IDeviceInfoQueries
    {
        /// <summary>
        /// 根据Id获取设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task<DeviceInfoDto> GetDeviceInfoByIdAsync(long id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            var info = await context.DeviceInfo.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (info is null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            var deviceModel = await context.DeviceModel.FirstAsync(w => w.Id == info.DeviceModelId);
            var groupList = await context.Groups.Include(i => i.Devices).Where(w => w.Devices.Select(s => s.DeviceInfoId).Contains(info.Id)).ToListAsync();
            var dto = mapper.Map<DeviceInfoDto>(info);
            dto.DeviceModelText = deviceModel?.Name;
            dto.GroupIds = groupList.Select(s => s.Id).ToList();
            dto.GroupsText = string.Join(",", groupList.Select(s => s.Name).ToList());
            dto.SettingInfo = mapper.Map<SettingInfoDto>(await context.SettingInfo.Include(i => i.MaterialBoxs).FirstOrDefaultAsync(x => x.DeviceId == info.Id));
            return dto;
        }

        /// <summary>
        /// 获取平台设备分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<DeviceListDto>> GetDevicePageListAsync(DeviceListInput request)
        {
            var baseInfo = from a in context.DeviceBaseInfo
                           join b in context.DeviceInfo on a.Id equals b.DeviceBaseId into joined
                           from b in joined.DefaultIfEmpty()
                           join c in context.EnterpriseInfo on b.EnterpriseinfoId equals c.Id into cJoin
                           from c in cJoin.DefaultIfEmpty()
                           join d in context.DeviceModel on a.DeviceModelId equals d.Id into dJoin
                           from d in dJoin.DefaultIfEmpty()
                           select new DeviceListDto
                           {
                               Id = a.Id,
                               Mid = a.Mid,
                               DeviceModelId = a.DeviceModelId,
                               EquipmentNumber = a.MachineStickerCode, //是否是唯一的那个码
                               Name = b.Name,
                               EnterpriseName = c.Name,
                               IsOnline = a.IsOnline,
                               DeviceModelName = d.Name,
                               CountryName = b.CountryRegionText,
                               RegionName = b.CountryRegionText,
                               DetaileAddress = b.DetailedAddress,
                               RegisterTime = b.CreateTime,
                               LatestOfflineTime = a.UpdateOfflineTime,
                               LatestOnlineTime = a.UpdateOnlineTime,
                               ActiveTime = b.ActiveTime,
                               CreateTime = b.CreateTime,
                               UpdateOfflineTime = a.UpdateOfflineTime,
                               UpdateOnlineTime = a.UpdateOnlineTime
                           };

            var baseInfoPage = await baseInfo
                .WhereIf(!string.IsNullOrEmpty(request.EquipmentNumber), a => a.EquipmentNumber.Contains(request.EquipmentNumber))
                .WhereIf(!string.IsNullOrEmpty(request.DeviceName), a => a.Name.Contains(request.DeviceName))
                .WhereIf(request.Status != null, a => a.IsOnline == request.Status)
                .WhereIf(request.DeviceModelId != null, a => a.DeviceModelId == request.DeviceModelId)
                .WhereIf(request.OnlineData != null && request.OnlineData.Count == 2, w => w.LatestOnlineTime >= request.OnlineData[0] && w.LatestOnlineTime <= request.OnlineData[1])
                .ToPagedListAsync(request);

            return baseInfoPage;

            request.IsIncludeQueries = true;
            long? enterpriseinfoId = null;
            if (!string.IsNullOrWhiteSpace(request.EnterpriseName))
            {
                var enterpriseInfo = await context.EnterpriseInfo.AsNoTracking().FirstOrDefaultAsync(w => w.Name.Contains(request.EnterpriseName));
                if (enterpriseInfo != null)
                    enterpriseinfoId = enterpriseInfo.Id;
            }
            var query = context.DeviceInfo.AsQueryable();
            //query = query.WhereIf(!string.IsNullOrWhiteSpace(request.EquipmentNumber), w => w.EquipmentNumber == request.EquipmentNumber)
            //    .WhereIf(!string.IsNullOrWhiteSpace(request.DeviceName), w => w.Name.Contains(request.DeviceName))
            //    .WhereIf(request.Status != null && request.Status != 0, w => (w.DeviceMetrics & request.Status) != 0)//查询包含当前搜索状态的数据
            //    .WhereIf(request.Status != null && request.Status == 0, w => w.DeviceMetrics == DeviceStatusEnum.Offline)
            //    .WhereIf(enterpriseinfoId != null, w => w.EnterpriseinfoId == enterpriseinfoId)
            //    .WhereIf(request.DeviceModelId != null, w => w.DeviceModelId == request.DeviceModelId);
            //分页查询
            var pageList = await query.AsNoTracking().ToPagedListAsync(request, "DeviceModel", "EnterpriseDevices");
            //获取当前设备企业信息
            var curAllEnterpriseList = await context.EnterpriseInfo.Where(w => pageList.Items.Select(s => s.EnterpriseinfoId).Contains(w.Id)).AsNoTracking().ToListAsync();
            //获取当前设备国家信息
            //var curAllCountryList = await context.CountryInfo.Where(w => pageList.Items.Select(s => s.CountryId).Contains(w.Id)).AsNoTracking().ToListAsync();
            try
            {
                PagedResultDto<DeviceListDto> pagedResultDto = new PagedResultDto<DeviceListDto>()
                {
                    Items = pageList.Items.Select(s => new DeviceListDto
                    {
                        //Id = s.Id,
                        //EquipmentNumber = s.EquipmentNumber,
                        //Name = s.Name,
                        //DeviceModelName = s.DeviceModel.Name,
                        //EnterpriseName = curAllEnterpriseList.FirstOrDefault(w => w.Id == s.EnterpriseinfoId)?.Name,
                        //DeviceMetrics = s.DeviceMetrics,
                        //Status = EnumExtensions.GetDescriptionsByInt<DeviceStatusEnum>((int)s.DeviceMetrics),
                        //CountryName = curAllCountryList.FirstOrDefault(w => w.Id == s.CountryId)?.CountryName,
                        //RegionName = s.CountryRegionText,
                        //DetailedAddress = s.DetailedAddress,
                        //RegisterTime = s.CreateTime.ToString("G"),
                        //LatestOnlineTime = s.LatestOnlineTime?.ToString("G"),
                        //LatestOfflineTime = s.LatestOfflineTime?.ToString("G")
                    }).ToList(),
                    PageNumber = pageList.PageNumber,
                    PageSize = pageList.PageSize,
                    TotalCount = pageList.TotalCount
                };
                return pagedResultDto;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// 根据Id获取设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<DeviceInfoDto> GetDeviceInfoAsync(long id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            var info = await context.DeviceInfo.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (info is null)
                throw ExceptionHelper.AppFriendly(L.Text[nameof(ErrorCodeEnum.D0014)]);
            var deviceModel = await context.DeviceModel.AsNoTracking().FirstOrDefaultAsync(w => w.Id == info.DeviceModelId);
            var groupList = await context.Groups.AsNoTracking().Include(i => i.Devices).Where(w => w.Devices.Select(s => s.DeviceInfoId).Contains(info.Id)).ToListAsync();
            var dto = mapper.Map<DeviceInfoDto>(info);
            dto.DeviceModelText = deviceModel?.Name;
            dto.GroupIds = groupList.Select(s => s.Id).ToList();
            dto.GroupsText = string.Join(",", groupList.Select(s => s.Name).ToList());
            dto.SettingInfo = mapper.Map<SettingInfoDto>(await context.SettingInfo.AsNoTracking().Include(i => i.MaterialBoxs).FirstOrDefaultAsync(x => x.DeviceId == info.Id));
            return dto;
        }
        /// <summary>
        /// 根据设备Id获取设备信息
        /// </summary>
        /// <param name="mids"></param>
        /// <returns></returns>
        public async Task<List<DeviceInfo>> GetDeviceInfoByMids(List<string> mids)
        {
            return await context.DeviceInfo.AsNoTracking().Where(x => mids.Contains(x.Mid)).ToListAsync();
        }

        /// <summary>
        /// 获取设备基本信息根据盒子id
        /// </summary>
        /// <param name="boxId"></param>
        /// <returns></returns>
        public async Task<DeviceBaseInfo> GetDeviceBaseInfoByBoxIdAsync(string boxId)
        {
            return await context.DeviceBaseInfo.FirstOrDefaultAsync(x => x.BoxId == boxId);
        }

        /// <summary>
        /// GetDeviceInitializationAsync
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public async Task<DeviceInitialization> GetDeviceInitializationAsync(string mid)
        {
            return await context.DeviceInitialization.FirstOrDefaultAsync(x => x.Mid == mid);
        }
    }
}
